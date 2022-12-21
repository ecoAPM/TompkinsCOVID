namespace TompkinsCOVID;

public interface ITwitter
{
	Task<DateOnly?> GetLatestPostedDate(string username);
	Task Tweet(Record record);
}