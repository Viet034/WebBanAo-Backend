using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using WebBanAoo.Data;
using WebBanAoo.Mapper;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Cart;
using WebBanAoo.Models.DTO.Request.Order;
using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Models.Status;
using WebBanAoo.Ultility;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Service.impl
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private IOrderMapper _mapper;
        private readonly Validation<Order> _validation;
        private readonly IOrderDetailService _orderDetail;

        public OrderService(ApplicationDbContext context, IOrderMapper mapper, Validation<Order> validation, IOrderDetailService orderDetail)
        {
            _context = context;
            _mapper = mapper;
            _validation = validation;
            _orderDetail = orderDetail;
        }

        public async Task<string> CheckUniqueCodeAsync()
        {
            string newCode;
            bool isExist;

            do
            {
                newCode = GenerateCode.GenerateOrderCode();
                _context.ChangeTracker.Clear();
                isExist = await _context.Orders.AnyAsync(p => p.Code == newCode);
            }
            while (isExist);

            return newCode;
        }

        public async Task<OrderResponse> CreateOrderAsync(OrderCreate create)
        {
            Order entity = _mapper.CreateToEntity(create);

            if (string.IsNullOrEmpty(entity.Code) || entity.Code == "string")
            {
                entity.Code = await CheckUniqueCodeAsync();
            }
            while (await _context.Orders.AnyAsync(p => p.Code == entity.Code))
            {
                entity.Code = await CheckUniqueCodeAsync();
            }

            await _context.Orders.AddAsync(entity);

            await _context.SaveChangesAsync();
            var response = _mapper.EntityToResponse(entity);
            return response;
        }

        public async Task<OrderResponse> FindOrderByIdAsync(int id)
        {
            var coId = await  _context.Orders.FindAsync( id);
            if (coId == null)
            {
                throw new KeyNotFoundException($" Không có Id {id} tồn tại");
            }
            var response = _mapper.EntityToResponse(coId);
            return response;
        }
        public async Task<IEnumerable<OrderResponse>> GetOrderByCustomerIdAsync(int id)
        {
            var tId = await _context.Orders.Where(pr => pr.CustomerId == id).OrderByDescending(x => x.OrderDate).ToListAsync();
            if (!tId.Any())
            {
                throw new Exception($"Không có Order nào thuộc id = {id}.");
            }
            var response = _mapper.ListEntityToResponse(tId);
            return response;
        }

        public async Task<IEnumerable<OrderResponse>> GetAllOrderAsync()
        {
            var co = await _context.Orders.OrderByDescending(x => x.OrderDate).ToListAsync();
            if (co == null) throw new Exception($"Không có bản ghi nào");

            var response = _mapper.ListEntityToResponse(co);

            return response;
        }

        public async Task<bool> HardDeleteOrderAsync(int id)
        {
            var co = await _context.Orders.FindAsync( id);
            if (co == null)
            {
                throw new KeyNotFoundException($"Không có Id {id} ton tai");
            }
            _context.Orders.Remove(co);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<OrderResponse>> SearchOrderByKeyAsync(string key)
        {
            var coKey = await _context.Orders
               .FromSqlRaw("Select * from Orders where Code like {0}", "%" + key + "%").ToListAsync();

            if (coKey == null) throw new Exception($"Không có Code {key} nào");
            var response = _mapper.ListEntityToResponse(coKey);
            return response;
        }

        public async Task<OrderResponse> SoftDeleteOrderAsync(int id, Status.OrderStatus newStatus)
        {
            var order = await _context.Orders
        .Include(o => o.Employee)
        .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                throw new KeyNotFoundException($"Đơn {id} không tồn tại");

           

            // Validate trạng thái
            if (!IsValidStatusTransition(order.Status, newStatus))
                throw new InvalidOperationException($"Không thể thay đổi từ trạng thái {order.Status} sang {newStatus}");

            // Cập nhật trạng thái
            order.Status = newStatus;
            order.UpdateDate = DateTime.Now;
            order.UpdateBy = order.Employee.FullName;

            await _context.SaveChangesAsync();
            return _mapper.EntityToResponse(order);
        }

        public async Task<OrderResponse> UpdateOrderAsync(int id, OrderUpdate update)
        {
            var coId = await _context.Orders.FindAsync( id);
            if (coId == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
            }
            coId.Code = await _validation.CheckAndUpdateAPIAsync(coId, coId.Code, update.Code, co => co.Code == update.Code);
            coId.Note = await _validation.CheckAndUpdateAPIAsync(coId, coId.Note, update.Note, co => co.Note == update.Note);
            coId.InitialTotalAmount = await _validation.CheckAndUpdatePriceAsync(coId, coId.InitialTotalAmount, update.InitialTotalAmount, co => co.InitialTotalAmount == update.InitialTotalAmount);
            coId.TotalAmount = await _validation.CheckAndUpdatePriceAsync(coId, coId.TotalAmount, update.TotalAmount, co => co.TotalAmount == update.TotalAmount);
            
            var result = _mapper.UpdateToEntity(update);
            
            coId.Status = result.Status;
            
            coId.OrderDate = result.OrderDate;
            coId.CreateDate = result.CreateDate;
            coId.UpdateDate = result.UpdateDate;
            coId.CreatedBy = result.CreatedBy;
            coId.UpdateBy = result.UpdateBy;


            await _context.SaveChangesAsync();

            var response = _mapper.EntityToResponse(coId);
            return response;
        }

        public async Task<OrderResponse> ApplyVoucherToOrderAsync(int orderId, int voucherId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == orderId);
            
            if (order == null)
                throw new KeyNotFoundException($"Đơn {orderId} không tồn tại!");

            var voucher = await _context.Vouchers
                .FirstOrDefaultAsync(v => v.Id == voucherId);
            
            if (voucher == null)
                throw new KeyNotFoundException($"Voucher {voucherId} không tồn tại!");

            // Kiểm tra các điều kiện
            if (voucher.Status != VoucherStatus.Active)
                throw new InvalidOperationException("Voucher chưa được kích hoạt!");
            
            if (voucher.StartDate > DateTime.Now || voucher.EndDate < DateTime.Now)
                throw new InvalidOperationException("Voucher đã hết hạn hoặc chưa đến hạn tạo.");
            
            if (order.InitialTotalAmount < voucher.MinimumOrderValue)
                throw new InvalidOperationException($"Đơn tối thiểu {voucher.MinimumOrderValue}");

            if (voucher.Quantity <= 0)
                throw new InvalidOperationException("Voucher đã hết!");

            // Kiểm tra xem khách hàng đã sử dụng voucher này chưa
            var customerVoucher = await _context.Customer_Vouchers
                .FirstOrDefaultAsync(cv => 
                    cv.CustomerId == order.CustomerId && 
                    cv.VoucherId == voucherId);

            if (customerVoucher?.Status == CustomerVoucherStatus.Used)
                throw new InvalidOperationException("Voucher này đã được sử dụng!");

            // Tính toán giảm giá
            decimal discount = Math.Min(
                order.InitialTotalAmount * voucher.DiscountValue / 100,
                voucher.MaxDiscount
            );

            // Cập nhật đơn hàng
            order.VoucherId = voucherId;
            order.TotalAmount = order.InitialTotalAmount - discount;
            
            // Cập nhật số lượng voucher
            voucher.Quantity--;
            
            // Cập nhật trạng thái sử dụng voucher của khách hàng
            if (customerVoucher == null)
            {
                customerVoucher = new Customer_Voucher
                {
                    CustomerId = order.CustomerId,
                    VoucherId = voucherId,
                    Status = CustomerVoucherStatus.Used,
                    
                };
                await _context.Customer_Vouchers.AddAsync(customerVoucher);
            }
            else
            {
                customerVoucher.Status = CustomerVoucherStatus.Used;
                
            }
            
            await _context.SaveChangesAsync();
            
            return _mapper.EntityToResponse(order);
        }

        public async Task<OrderResponse> CheckoutFromCartAsync(CartCheckoutRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Lấy hoặc tạo Employee System
                var systemEmployee = await _context.Employees
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Id == 29)
                    ?? await CreateSystemEmployee();

                // Truy vấn giỏ hàng trước
                var cart = await _context.Carts
                    .FirstOrDefaultAsync(x => x.CustomerId == request.CustomerId);

                if (cart == null)
                {
                    throw new Exception("Giỏ hàng không tồn tại.");
                }

                // Load Cart_ProductDetails riêng
                var cartDetails = await _context.Cart_ProductDetails
                    .Where(x => x.CartId == cart.CartId)
                    .ToListAsync();

                if (!cartDetails.Any())
                {
                    throw new Exception("Giỏ hàng rỗng.");
                }

                cart.Cart_ProductDetails = cartDetails;
                var listIDProductDetail = cartDetails.Select(x => x.ProductDetailId).ToHashSet();

                // Kiểm tra số lượng trước khi thanh toán
                await ValidateProductQuantitiesAsync(cart);

                // Tạo Order mới
                var order = new Order
                {
                    CustomerId = request.CustomerId,
                    EmployeeId = systemEmployee.Id,
                    Status = OrderStatus.Pending,
                    Note = request.Note,
                    Code = await CheckUniqueCodeAsync(),
                    VoucherId = request.VoucherId,
                    OrderDate = DateTime.Now,
                    CreatedBy = "System",
                    CreateDate = DateTime.Now,
                    OrderDetails = new List<OrderDetail>() // Khởi tạo collection
                };

                // Lấy thông tin sản phẩm
                var productDetails = await _context.ProductDetail
                    .Where(pd => listIDProductDetail.Contains(pd.Id))
                    
                    .ToDictionaryAsync(pd => pd.Id, pd => pd);

                decimal totalAmount = 0;

                // Xử lý từng sản phẩm trong giỏ hàng
                foreach (var cartItem in cartDetails)  // Sử dụng cartDetails thay vì cart.Cart_ProductDetails
                {
                    if (!productDetails.TryGetValue(cartItem.ProductDetailId, out var productDetail))
                    {
                        throw new Exception($"Sản phẩm với Id {cartItem.ProductDetailId} không tồn tại");
                    }

                    if (productDetail.Quantity < cartItem.Quantity)
                    {
                        throw new Exception($"Số lượng của sản phẩm {productDetail.Id} đã hết");
                    }

                    // Tạo chi tiết đơn hàng
                    var orderDetail = new OrderDetail
                    {
                        Code = await _orderDetail.CheckUniqueCodeAsync(),
                        ProductDetailId = cartItem.ProductDetailId,
                        Quantity = cartItem.Quantity,
                        UnitPrice = productDetail.Price,
                        TotalAmount = productDetail.Price * cartItem.Quantity,
                        Status = OrderDetailStatus.Processed,
                        Discount = 0,
                        CreatedBy = "System",
                        CreateDate = DateTime.Now,
                        UpdateBy = "System",
                        UpdateDate = DateTime.Now
                    };

                    totalAmount += orderDetail.TotalAmount;
                    order.OrderDetails.Add(orderDetail);

                    // Cập nhật số lượng trong database trực tiếp
                    await _context.Database.ExecuteSqlRawAsync(
                        "UPDATE ProductDetails SET Quantity = Quantity - {0} WHERE Id = {1}",
                        cartItem.Quantity, cartItem.ProductDetailId);
                }

                order.InitialTotalAmount = totalAmount;
                order.TotalAmount = totalAmount;

                // Lưu đơn hàng
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                // Áp dụng voucher nếu có
                if (request.VoucherId.HasValue)
                {
                    try
                    {
                        await ApplyVoucherToOrderAsync(order.Id, request.VoucherId.Value);
                    }
                    catch (Exception voucherEx)
                    {
                        throw new Exception($"{voucherEx.Message}");
                       // order.Note += $"\nVoucher application failed: {voucherEx.Message}";
                        await _context.SaveChangesAsync();
                    }
                }
                
                // Xóa giỏ hàng bằng SQL trực tiếp
                await _context.Database.ExecuteSqlRawAsync(
                    "DELETE FROM Cart_ProductDetails WHERE CartId = {0}",
                    cart.CartId);

                await transaction.CommitAsync();
                return _mapper.EntityToResponse(order);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Lỗi khi thanh toán: {ex.Message}", ex);
            }

        }


        private async Task<Employee> CreateSystemEmployee()
        {
            var systemEmployee = new Employee
            {
                Code = "SYSTEM",
                FullName = "System",
                Email = "system@example.com",
                Password = "1",
                Gender = Gender.Male,
                Dob = DateTime.Now,
                CreatedBy = "System",
                CreateDate = DateTime.Now,
                Status = EmployeeStatus.Working,
                Phone = "0912345677",
                Address = "system",
                City = "system",
                StartDate = DateTime.Now,
                Image = "default-system-avatar.jpg"
            };

            await _context.Employees.AddAsync(systemEmployee);
            await _context.SaveChangesAsync();

            return systemEmployee;
        }
        public async Task<OrderResponse> AssignEmployeeToOrderAsync(int orderId, int employeeId)
        {
            var order = await _context.Orders
        .Include(o => o.Employee)
        .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new KeyNotFoundException($"Đơn hàng {orderId} không tồn tại");

            // Chỉ cho assign khi đơn đang ở System
            if (order.Employee.Id != 29)
                throw new InvalidOperationException("Đơn hàng đã được đăng ký bởi nhân viên khác");

            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null)
                throw new KeyNotFoundException($"Nhân viên {employeeId} không tồn tại");

            // Chỉ assign employee, không thay đổi status
            order.EmployeeId = employeeId;
            order.UpdateDate = DateTime.Now;
            order.UpdateBy = employee.FullName;

            await _context.SaveChangesAsync();
            return _mapper.EntityToResponse(order);
        }

        private bool IsValidStatusTransition(OrderStatus currentStatus, OrderStatus newStatus)
        {
            // Định nghĩa các chuyển đổi trạng thái hợp lệ
            switch (currentStatus)
            {
                case OrderStatus.Pending:
                    return newStatus == OrderStatus.Confirmed || newStatus == OrderStatus.Cancelled;

                case OrderStatus.Confirmed:
                    return newStatus == OrderStatus.Shipped || newStatus == OrderStatus.Cancelled;

                case OrderStatus.Shipped:
                    return newStatus == OrderStatus.Delivered || newStatus == OrderStatus.Cancelled;

                case OrderStatus.Delivered:
                    return false; // Không thể thay đổi sau khi đã delivered

                case OrderStatus.Cancelled:
                    return false; // Không thể thay đổi sau khi đã cancelled

                default:
                    return false;
            }
        }

        // Lấy danh sách đơn hàng chưa được assign (đang ở SYSTEM)
        public async Task<IEnumerable<OrderResponse>> GetPendingOrdersAsync()
        {
            var systemEmployee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == 29);

            if (systemEmployee == null)
                throw new InvalidOperationException("System employee not found");

            var pendingOrders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductDetail)
                .Where(o => o.EmployeeId == systemEmployee.Id)
                .OrderBy(o => o.CreateDate)
                .ToListAsync();

            return _mapper.ListEntityToResponse(pendingOrders);
        }

        // Lấy danh sách đơn hàng của nhân viên đang xử lý
        public async Task<IEnumerable<OrderResponse>> GetOrdersByEmployeeIdAsync(int employeeId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null)
                throw new KeyNotFoundException($"Employee {employeeId} not found");

            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductDetail)
                .Where(o => o.EmployeeId == employeeId)
                .OrderByDescending(o => o.CreateDate)
                .ToListAsync();

            return _mapper.ListEntityToResponse(orders);
        }

        private async Task ValidateProductQuantitiesAsync(Cart cart)
        {
            // Lấy danh sách ProductDetail IDs từ giỏ hàng
            var productDetailIds = cart.Cart_ProductDetails.Select(x => x.ProductDetailId).ToList();
            
            // Lấy thông tin ProductDetail từ database
            var productDetails = await _context.ProductDetail
                .Where(pd => productDetailIds.Contains(pd.Id))
                .ToDictionaryAsync(pd => pd.Id, pd => pd);

            // Danh sách sản phẩm không đủ số lượng
            var insufficientProducts = new List<string>();

            // Kiểm tra từng sản phẩm trong giỏ hàng
            foreach (var cartItem in cart.Cart_ProductDetails)
            {
                if (!productDetails.TryGetValue(cartItem.ProductDetailId, out var productDetail))
                {
                    throw new InvalidOperationException($"Product detail with ID {cartItem.ProductDetailId} not found");
                }

                // Kiểm tra số lượng tồn kho
                if (productDetail.Quantity == 0)
                {
                    insufficientProducts.Add($"Product {productDetail.Code} is out of stock");
                }
                else if (productDetail.Quantity < cartItem.Quantity)
                {
                    insufficientProducts.Add($"Product {productDetail.Code} has insufficient stock (Available: {productDetail.Quantity}, Required: {cartItem.Quantity})");
                }
            }

            // Nếu có sản phẩm không đủ số lượng, throw exception
            if (insufficientProducts.Any())
            {
                throw new InvalidOperationException(
                    "Cannot process checkout due to insufficient stock:\n" + 
                    string.Join("\n", insufficientProducts)
                );
            }
        }
    }
}
