﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toutokaz.Data.Interfaces;
using Toutokaz.Domain.Models;

namespace Toutokaz.Data.Repositories
{
    public class AnnoncesImageRepository: EntityRepository<tb_ad_image>, IAnnoncesImageRepository
    {
    }
}
