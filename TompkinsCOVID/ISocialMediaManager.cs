namespace TompkinsCOVID;

public interface ISocialMediaManager
{
	Task<DateOnly?> GetLatestPostedDate(string username);
	Task Post(Record record);
}