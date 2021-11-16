namespace TompkinsCOVID;

public interface IHealthDepartment
{
	Task<IList<Record>> GetLatestRecords(string url);
}