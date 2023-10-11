﻿using System;
using System.Collections.Generic;

namespace PurrfectpawsApi.Models;

public partial class MCategory
{
    public int CategoryId { get; set; }

    public string Category { get; set; } = null!;

    public virtual ICollection<TProductDetail> TProductDetails { get; set; } = new List<TProductDetail>();
}
