namespace TompkinsCOVID;

public interface ITwitter
{
	Task<DateTime?> GetLatestPostedDate(string username);
	Task Tweet(Record record);
}