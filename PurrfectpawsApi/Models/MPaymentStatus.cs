﻿using System;
using System.Collections.Generic;

namespace PurrfectpawsApi.Models;

public partial class MPaymentStatus
{
    public int PaymentStatusId { get; set; }

    public string PaymentStatus { get; set; } = null!;

    public virtual ICollection<TTransaction> TTransactions { get; set; } = new List<TTransaction>();
}
