using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardModel {
    [JsonIgnore]
    public long Id { get; set; }

    public long Score { get; set; }
    public string User { get; set; }
}
