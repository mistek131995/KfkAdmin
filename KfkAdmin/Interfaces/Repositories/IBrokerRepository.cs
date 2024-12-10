﻿using KfkAdmin.Interfaces.Common;
using KfkAdmin.Models.Entities;

namespace KfkAdmin.Interfaces.Repositories;

public interface IBrokerRepository : IBaseKafkaRepository
{
    List<Broker> GetAll();
}