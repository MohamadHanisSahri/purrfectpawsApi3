﻿using System;
using System.Collections.Generic;

namespace PurrfectpawsApi.Models;

public partial class MRole
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<TUser> TUsers { get; set; } = new List<TUser>();
}
