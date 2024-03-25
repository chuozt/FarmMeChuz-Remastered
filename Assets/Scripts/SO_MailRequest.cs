using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SO_MailRequest : ScriptableObject
{
    [field:SerializeField] public Item RequestItem { get; set; }
    [field:SerializeField] public int MinRequestNumber { get; set; }
    [field:SerializeField] public int MaxRequestNumber { get; set; }
    [field:SerializeField, TextArea] public string RequestReason { get; set; }
    [field:SerializeField] public string Signature { get; set; }
}
