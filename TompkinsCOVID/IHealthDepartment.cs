namespace TompkinsCOVID;

public interface IHealthDepartment
{
	Task<IDictionary<DateTime, Record>> GetRecords();
	Task<IDictionary<DateTime, Record>> GetRecordsSince(DateTime latest);
}