using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class HyperLinks : MonoBehaviour 
{
	public void OpenItchio()
	{
		Application.OpenURL("https://chuozt.itch.io/");
	}

	public void OpenYoutube()
	{
		Application.OpenURL("https://www.youtube.com/channel/UCx-hSID6IVm9HLUIRq-XbXQ");
	}

    public void OpenDiscord()
	{
		Application.OpenURL("https://discord.gg/657qkzJ3Wz");
	}

    public void OpenInstagram()
	{
		Application.OpenURL("https://www.instagram.com/chuozt/");
	}

	public void OpenFacebook()
	{
		Application.OpenURL("https://www.facebook.com/profile.php?id=100087463626565");
	}
}