﻿using static WebBanAoo.Models.Status.Status;
using System.ComponentModel.DataAnnotations;

namespace WebBanAoo.Models.DTO.Request.Size
{
    public class SizeDelete
    {
        public int Id { get; set; }
  
        public string Code { get; set; }
 
        public SizeStatus Status { get; set; } = SizeStatus.Available;
        public SizeCode SizeCode { get; set; } = SizeCode.M;

        public SizeDelete()
        {
        }

        public SizeDelete(int id, string code, SizeStatus status, SizeCode sizeCode)
        {
            Id = id;
            Code = code;
            Status = status;
            SizeCode = sizeCode;
        }
    }
}
