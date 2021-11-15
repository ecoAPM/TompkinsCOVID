using System;
using System.Threading.Tasks;

namespace TompkinsCOVID;

public interface ITwitter
{
	Task<DateTime?> GetLatestPostedDate();
	Task Tweet(Record record);
}
