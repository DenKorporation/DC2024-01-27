﻿using System;
using System.Collections.Generic;

namespace DC_Lab1.Models;

public partial class Editor
{
    public int Id { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public virtual ICollection<Tweet> Tweets { get; set; } = new List<Tweet>();
}
