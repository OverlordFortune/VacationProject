﻿using System.Linq;
using VacationsBLL.DTOs;
using VacationsBLL.Functions;
using VacationsBLL.Interfaces;
using VacationsDAL.Entities;
using VacationsDAL.Interfaces;

namespace VacationsBLL.Services
{
    public  class ProfileDataService :  IProfileDataService
    {
        private const string empty = "None";
        private IEmployeeRepository _employees;

        private IJobTitleRepository _jobTitles;

        private IVacationRepository _vacations;

        private IVacationStatusTypeRepository _vacationStatusTypes;

        private IVacationTypeRepository _vacationTypes;


        public ProfileDataService(IEmployeeRepository employees,
                                  IJobTitleRepository jobTitles,
                                  IVacationRepository vacations,
                                  IVacationStatusTypeRepository vacationStatusTypes,
                                  IVacationTypeRepository vacationTypes)
        {
            _employees = employees;
            _jobTitles = jobTitles;
            _vacations = vacations;
            _vacationTypes = vacationTypes;
            _vacationStatusTypes = vacationStatusTypes;
        }

        public ProfileDTO GetUserData(string id)
        {

            var employee = _employees.GetById(id);

            var jobTitle = _jobTitles.GetById(employee.JobTitleID).JobTitleName;

            if (employee != null)
            {
                var userData = Mapper.Map<Employee, ProfileDTO>(employee);
                userData.TeamName = employee.EmployeesTeam.Count.Equals(0) ? empty : employee.EmployeesTeam.First().TeamName;
                userData.TeamLeader = employee.EmployeesTeam.Count.Equals(0) ? empty : string.Format($"{ _employees.Get(x => x.EmployeeID.Equals(employee.EmployeesTeam.First().TeamLeadID)).First().Name}" +
                                                                                                      $"{ _employees.Get(x => x.EmployeeID.Equals(employee.EmployeesTeam.First().TeamLeadID)).First().Surname}");
                userData.JobTitle = jobTitle;
                userData.EmployeeID = employee.EmployeeID;
                return userData;
            }

            return null;         
        }

        public ProfileVacationDTO[] GetUserVacationsData(string id)
        {
            var employee = _employees.GetById(id);

            var vacations = _vacations.Get(x => x.EmployeeID.Equals(employee.EmployeeID)).Select(x => new ProfileVacationDTO
            {
                VacationType = _vacationTypes.GetById(x.VacationTypeID).VacationTypeName,
                VacationStatusType = _vacationStatusTypes.GetById(x.VacationStatusTypeID).VacationStatusName,
                Comment = x.Comment,
                DateOfBegin = x.DateOfBegin,
                DateOfEnd = x.DateOfEnd,
                Duration = x.Duration,
                Status = _vacationStatusTypes.GetById(x.VacationStatusTypeID).VacationStatusName,
                Created = x.Created
            }).OrderBy(req => FunctionHelper.VacationSortFunc(req.Status)).ThenBy(req => req.Created).ToArray();

            return vacations;
        }

        public VacationBalanceDTO GetUserVacationBalance(string id)
        {
            var employee = _employees.GetById(id);

            return new VacationBalanceDTO { Balance = employee.VacationBalance };
        }
    }
}
