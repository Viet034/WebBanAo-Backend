﻿using static WebBanAoo.Models.Status.Status;
using System.ComponentModel.DataAnnotations;

namespace WebBanAoo.Models.DTO.Request.Color
{
    public class ColorUpdate
    {
        public int Id { get; set; }
        
        public string Code { get; set; }

        public string ColorName { get; set; }
        public ColorStatus Status { get; set; } = ColorStatus.Available;

        public ColorUpdate()
        {
        }

        public ColorUpdate(int id, string code, string colorName, ColorStatus status)
        {
            Id = id;
            Code = code;
            ColorName = colorName;
            Status = status;
        }
    }
}
