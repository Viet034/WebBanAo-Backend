using System.Text.Json.Serialization;

namespace WebBanAoo.Models.Status
{
    public static class Status
    {
        public enum Gender
        {
            Male = 0, Female = 1
        }
        //public enum BrandStatus
        //{
        //    Active = 0, Inactive = 1, Remove = 2
        //}
        public enum CartProductDetailStatus
        {
            Available = 0, OutOfStock = 1, Remove = 2
        }

        public enum CategoryStatus
        {
            Active = 0, Inactive = 1, Hidden = 2
        }

        public enum ColorStatus
        {
            Available = 0, Unavailable = 1
        }
        public enum CustomerStatus
        {
            Active = 0, Banned = 1
        }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum CustomerVoucherStatus
        {
            Active = 0, Expired = 1, Used = 2
        }
  
        public enum EmployeeStatus
        {
            Working = 0, Not_Working = 1
        }

        //public enum EmployeeRoleStatus
        //{
        //    Active = 0, Inactive = 1
        //}
  
        public enum OrderStatus
        {
            Pending = 0, Confirmed = 1,Shipped = 2, Delivered = 3, Cancelled = 4
        }
        public enum OrderDetailStatus
        {
            Processed = 0, Pending = 1, Complete = 2
        }

        public enum ProductStatus
        {
            Available = 0, Unavailable = 1
        }
        public enum ProductDetailStatus
        {
            Available = 0, OutOfStock = 1
        }
        public enum ProductDetailSaleStatus
        {
            Ongoing = 0, Ended = 1, Scheduled = 2
        }
        public enum ProductImageStatus
        {
            Active = 0, Inactive = 1
        }
        public enum RoleStatus
        {
            Active = 0, Inactive = 1
        }
        public enum SaleStatus
        {
            Active = 0, Inactive = 1, Scheduled = 2
        }

        public enum SizeStatus
        {
            Available = 0, Unavailable = 1
        }
        public enum SizeCode
        {
            XS = 0, S = 1, M = 2, L = 3, XL = 4, XXL = 5, XXXL = 6
        }
        public enum VoucherStatus
        {
            Active = 0, Expired = 1, Used = 2
        }

    }
}
