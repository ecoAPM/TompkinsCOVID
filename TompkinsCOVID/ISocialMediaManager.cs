namespace TompkinsCOVID;

public interface ISocialMediaManager
{
	Task<DateOnly?> GetLatestPostedDate();
	Task Post(Record record);
}