using Dapper;
using QuickCare.Areas.Receptionist.Controllers.ViewModals;
using System.Data;

namespace QuickCare.Areas.Receptionist.Repositories
{
    public class PatientRepository
    {
        private readonly IDbConnection _conn;
        public PatientRepository(IDbConnection conn) { _conn = conn; }

        public List<PatientVM> GetPatients(int orgId, string search, int pageNumber, int pageSize = 10)
        {
            string query = @"
                SELECT p.*, c.CityName, b.BloodGroup
                FROM QC.Patients p
                LEFT JOIN Data.City c ON p.CityId = c.CityId
                LEFT JOIN Data.BloodGroups b ON p.BloodGroupId = b.BloodGroupId
                WHERE p.OrganizationId = @orgId 
                AND (@search = '' OR p.FullName LIKE @search OR p.PhoneNumber LIKE @search)
                ORDER BY p.PatientId DESC
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            return _conn.Query<PatientVM>(query, new
            {
                orgId,
                search = $"%{search}%",
                Offset = (pageNumber - 1) * pageSize,
                PageSize = pageSize
            }).ToList();
        }

        public async Task<int> UpsertPatient(PatientVM p, int userId)
        {
            string query = @"
                IF EXISTS (SELECT 1 FROM QC.Patients WHERE PatientId = @PatientId)
                BEGIN
                    UPDATE QC.Patients SET FullName=@FullName, Gender=@Gender, DateOfBirth=@DateOfBirth, 
                    PhoneNumber=@PhoneNumber, Email=@Email, Address=@Address, CityId=@CityId, 
                    BloodGroupId=@BloodGroupId, Allergies=@Allergies, MedicalHistory=@MedicalHistory,
                    ModifiedBy=@userId, ModifiedOn=GETDATE() WHERE PatientId=@PatientId
                END
                ELSE
                BEGIN
                    INSERT INTO QC.Patients (FullName, Gender, DateOfBirth, PhoneNumber, Email, Address, CityId, BloodGroupId, Allergies, MedicalHistory, OrganizationId, CreatedBy, CreatedOn, IsActive)
                    VALUES (@FullName, @Gender, @DateOfBirth, @PhoneNumber, @Email, @Address, @CityId, @BloodGroupId, @Allergies, @MedicalHistory, @OrganizationId, @userId, GETDATE(), 1)
                END";
            return await _conn.ExecuteAsync(query, new
            {
                p.PatientId,
                p.FullName,
                p.Gender,
                p.DateOfBirth,
                p.PhoneNumber,
                p.Email,
                p.Address,
                p.CityId,
                p.BloodGroupId,
                p.Allergies,
                p.MedicalHistory,
                p.OrganizationId,
                userId
            });
        }

        public async Task<int> DeletePatient(int id) =>
            await _conn.ExecuteAsync("UPDATE QC.Patients SET IsActive=0 WHERE PatientId=@id", new { id });
    }
}