﻿using Newtonsoft.Json;
using System;

namespace challenge.Models
{
    public class Compensation
    {
        public Employee Employee { get; set; }
        public decimal Salary { get; set; }
        public DateTime EffectiveDate { get; set; }
        public String EmployeeId { get; set; }

        [JsonIgnore]
        public String CompensationId { get; set; }
    }
}
