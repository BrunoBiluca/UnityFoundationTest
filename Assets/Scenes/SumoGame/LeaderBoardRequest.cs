using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderBoardRequest : Singleton<LeaderBoardRequest>
{
    void Start()
    {
        var leaderboard = new LeaderBoardModel() {
            Score = UnityEngine.Random.Range(1, 1000),
            User = Guid.NewGuid().ToString()
        };

        Debug.Log(JsonConvert.SerializeObject(
            leaderboard,
            new JsonSerializerSettings() {
                ContractResolver = new DefaultContractResolver {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            }
        ));

        WebRequests.Post(
            "https://localhost:44376/api/leaderboards",
            JsonConvert.SerializeObject(leaderboard),
            (response) => {
                Debug.Log(JsonConvert.DeserializeObject<LeaderBoardModel>(response).Score);
                WebRequests.Get(
                    "https://localhost:44376/api/leaderboards",
                    (response) => Debug.Log(
                        JsonConvert.DeserializeObject<List<LeaderBoardModel>>(response).Last().User),
                    (error) => Debug.Log(error)
                );
            },
            (error) => Debug.Log(error)
        );

    }
}
