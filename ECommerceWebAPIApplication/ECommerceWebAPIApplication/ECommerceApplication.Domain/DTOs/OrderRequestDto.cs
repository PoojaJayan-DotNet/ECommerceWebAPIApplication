﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApplication.Domain.DTOs
{
    public class OrderRequestDto
    {
        public int CutomerId { get; set; }
        public string Email { get; set; }
    }
}
