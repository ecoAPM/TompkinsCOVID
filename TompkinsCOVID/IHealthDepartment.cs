namespace TompkinsCOVID;

public interface IHealthDepartment
{
	Task<IDictionary<DateOnly, Record>> GetRecords();
	Task<IDictionary<DateOnly, Record>> GetRecordsSince(DateOnly latest);
}