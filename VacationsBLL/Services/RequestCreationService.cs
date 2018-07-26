﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationsBLL.Interfaces;
using VacationsDAL.Interfaces;
using VacationsBLL.DTOs;
using VacationsDAL.Entities;

namespace VacationsBLL.Services
{
    public class RequestCreationService : IRequestCreationService
    {
        private IVacationRepository _vacations;

        private IVacationStatusTypeRepository _vacationStatusTypes;

        public string GetStatusIdByType(string type)
        {
            return _vacationStatusTypes.GetByType(type).VacationStatusTypeID;
        }

        public RequestCreationService(IVacationRepository vacations, IVacationStatusTypeRepository vacationStatusTypes)
        {
            _vacations = vacations;
            _vacationStatusTypes = vacationStatusTypes;
        }

        public void CreateVacation(VacationDTO vacation)
        {
            _vacations.Add(Mapper.Map<VacationDTO, Vacation>(vacation));
        }

        public void Dispose()
        {
            _vacations.Dispose();
            _vacationStatusTypes.Dispose();
        }
    }
}
