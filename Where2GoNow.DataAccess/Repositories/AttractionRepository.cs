using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Util;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TripAdvisor;

namespace Where2GoNow.DataAccess.Repositories
{
    public class AttractionRepository
    {
        private const string TableName = "attractions";
        private const double Degrees2MilesRatio = 50.0;

        private IDynamoDBContext _dbContext { get; set; }

        public AttractionRepository()
        {
            AWSConfigsDynamoDB.Context.TypeMappings[typeof(Attraction)] = new TypeMapping(typeof(Attraction), "attractions");
            Amazon.DynamoDBv2.DataModel.DynamoDBContextConfig config = new Amazon.DynamoDBv2.DataModel.DynamoDBContextConfig()
            {
                Conversion = DynamoDBEntryConversion.V2
            };
            this._dbContext = (IDynamoDBContext)new DynamoDBContext((IAmazonDynamoDB)new AmazonDynamoDBClient(RegionEndpoint.USWest2), config);
        }

        public async Task<IEnumerable<Attraction>> GetAttractions(
          double lat,
          double lng,
          double radius,
          int popularity)
        {
            double num = radius / 50.0;
            return (IEnumerable<Attraction>)await this._dbContext.ScanAsync<Attraction>((IEnumerable<ScanCondition>)new ScanCondition[3]
            {
        new ScanCondition(nameof (lat), ScanOperator.Between, new object[2]
        {
          (object) (lat - num),
          (object) (lat + num)
        }),
        new ScanCondition(nameof (lng), ScanOperator.Between, new object[2]
        {
          (object) (lng - num),
          (object) (lng + num)
        }),
        new ScanCondition("reviews", ScanOperator.GreaterThanOrEqual, new object[1]
        {
          (object) popularity
        })
            }).GetRemainingAsync(new CancellationToken());
        }

        public async Task InsertAttractions(IEnumerable<Attraction> attractions)
        {
            BatchWrite<Attraction> batchWrite = this._dbContext.CreateBatchWrite<Attraction>();
            batchWrite.AddPutItems(attractions);
            await batchWrite.ExecuteAsync(new CancellationToken());
        }
    }
}

