using ExaminationSystem.Enums;
using ExaminationSystem.Enums.JWT_Role;
using ExaminationSystem.Models;
using ExaminationSystem.Repo;
using ExaminationSystem.ViewModels;

namespace ExaminationSystem.Services
{
    public class RoleFeatureService
    {
        private readonly GenericRepository<RoleFeature> _RoleFeatureRepo;
        public RoleFeatureService(GenericRepository<RoleFeature> RoleFeatureRepo)
        {
            _RoleFeatureRepo = RoleFeatureRepo;
        }

        public async Task<ResponseViewModel<bool>> AssignFeatureToRoleAsync(Feature feature,Role role)
        {
            //validate feature role
            if (!Enum.IsDefined(typeof(Role), role))
                return ResponseViewModel<bool>.Failure(ErrorCode.InvalidRole, message: "Invalid role specified.");

            if (!Enum.IsDefined(typeof(Feature), feature))
                return ResponseViewModel<bool>.Failure(ErrorCode.InvalidFeature, message: "Invalid feature specified.");

            //validate feature not assigned twice to this role
            bool isAlreadyAssigned = await _RoleFeatureRepo.AnyAsync(rf => rf.Role == role && rf.Feature == feature);
            if (isAlreadyAssigned)
                return ResponseViewModel<bool>.Failure(ErrorCode.FeatureAlreadyAssignedToRole, message: "Feature is already assigned to this role.");

            var result = await _RoleFeatureRepo.AddAsync(new RoleFeature() {Feature=feature,Role=role});

            if (result)
                return ResponseViewModel<bool>.Success(result, ErrorCode.None, message: "Feature Assigned to role Successfully");
            else
                return ResponseViewModel<bool>.Failure(ErrorCode.AssignFeatureToRoleFail, message: "Fail to assign feature");
        }

        //public async Task<ResponseViewModel<bool>> HasAccess(Feature feature, Role role)
        //{

        //}
    }
}
