using System.Diagnostics;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

/// <summary>
/// These super simple and stupid tests allow you to see how slower Obscured types can be compared to the regular types.
/// Take in account iterations count though.
/// </summary>
public class PerformanceObscuredTests : MonoBehaviour
{
	public bool boolTest = true;
	public int boolIterations = 2500000;

	public bool byteTest = true;
	public int byteIterations = 2500000;

	public bool shortTest = true;
	public int shortIterations = 2500000;

	public bool ushortTest = true;
	public int ushortIterations = 2500000;

	public bool intTest = true;
	public int intIterations = 2500000;

	public bool uintTest = true;
	public int uintIterations = 2500000;

	public bool longTest = true;
	public int longIterations = 2500000;

	public bool floatTest = true;
	public int floatIterations = 2500000;

	public bool doubleTest = true;
	public int doubleIterations = 2500000;

	public bool stringTest = true;
	public int stringIterations = 250000;

	public bool vector3Test = true;
	public int vector3Iterations = 2500000;

	public bool prefsTest = true;
	public int prefsIterations = 2500;

	private void Start()
	{
		Invoke("StartTests", 1f);
	}

	private void StartTests()
	{
		if (boolTest) TestBool();
		if (byteTest) TestByte();
		if (shortTest) TestShort();
		if (ushortTest) TestUShort();
		if (intTest) TestInt();
		if (uintTest) TestUInt();
		if (longTest) TestLong();
		if (floatTest) TestFloat();
		if (doubleTest) TestDouble();
		if (stringTest) TestString();
		if (vector3Test) TestVector3();
		if (prefsTest) TestPrefs();
	}

	private void TestBool()
	{
		Stopwatch sw = Stopwatch.StartNew();

		ObscuredBool obscured = true;
		bool notObscured = obscured;
		bool dummy = false;

		for (int i = 0; i < boolIterations; i++)
		{
			dummy = obscured;
		}

		for (int i = 0; i < boolIterations; i++)
		{
			obscured = dummy;
		}

		sw.Reset();

		sw.Start();
		for (int i = 0; i < boolIterations; i++)
		{
			dummy = notObscured;
		}

		for (int i = 0; i < boolIterations; i++)
		{
			notObscured = dummy;
		}

		sw.Stop();

		if (dummy){}
		if (obscured) { }
		if (notObscured) { }
	}

	private void TestByte()
	{

		Stopwatch sw = Stopwatch.StartNew();

		ObscuredByte obscured = 100;
		byte notObscured = obscured;
		byte dummy = 0;

		for (int i = 0; i < byteIterations; i++)
		{
			dummy = obscured;
		}

		for (int i = 0; i < byteIterations; i++)
		{
			obscured = dummy;
		}

		sw.Reset();

		sw.Start();
		for (int i = 0; i < byteIterations; i++)
		{
			dummy = notObscured;
		}

		for (int i = 0; i < byteIterations; i++)
		{
			notObscured = dummy;
		}

		sw.Stop();

		if (dummy != 0) { }
		if (obscured != 0) { }
		if (notObscured != 0) { }
	}

	private void TestShort()
	{

		Stopwatch sw = Stopwatch.StartNew();

		ObscuredShort obscured = 100;
		short notObscured = obscured;
		short dummy = 0;

		for (int i = 0; i < shortIterations; i++)
		{
			dummy = obscured;
		}

		for (int i = 0; i < shortIterations; i++)
		{
			obscured = dummy;
		}

		sw.Reset();

		sw.Start();
		for (int i = 0; i < shortIterations; i++)
		{
			dummy = notObscured;
		}

		for (int i = 0; i < shortIterations; i++)
		{
			notObscured = dummy;
		}

		sw.Stop();

		if (dummy != 0) { }
		if (obscured != 0) { }
		if (notObscured != 0) { }
	}

	private void TestUShort()
	{

		Stopwatch sw = Stopwatch.StartNew();

		ObscuredUShort obscured = 100;
		ushort notObscured = obscured;
		ushort dummy = 0;

		for (int i = 0; i < ushortIterations; i++)
		{
			dummy = obscured;
		}

		for (int i = 0; i < ushortIterations; i++)
		{
			obscured = dummy;
		}

		sw.Reset();

		sw.Start();
		for (int i = 0; i < ushortIterations; i++)
		{
			dummy = notObscured;
		}

		for (int i = 0; i < ushortIterations; i++)
		{
			notObscured = dummy;
		}

		sw.Stop();

		if (dummy != 0) { }
		if (obscured != 0) { }
		if (notObscured != 0) { }
	}

	private void TestDouble()
	{

		Stopwatch sw = Stopwatch.StartNew();

		ObscuredDouble obscured = 100d;
		double notObscured = obscured;
		double dummy = 0;

		for (int i = 0; i < doubleIterations; i++)
		{
			dummy = obscured;
		}

		for (int i = 0; i < doubleIterations; i++)
		{
			obscured = dummy;
		}

		sw.Reset();

		sw.Start();
		for (int i = 0; i < doubleIterations; i++)
		{
			dummy = notObscured;
		}

		for (int i = 0; i < doubleIterations; i++)
		{
			notObscured = dummy;
		}

		sw.Stop();

		if (dummy != 0) { }
		if (obscured != 0) { }
		if (notObscured != 0) { }
	}

	private void TestFloat()
	{

		Stopwatch sw = Stopwatch.StartNew();

		ObscuredFloat obscured = 100f;
		float notObscured = obscured;
		float dummy = 0;

		for (int i = 0; i < floatIterations; i++)
		{
			dummy = obscured;
		}

		for (int i = 0; i < floatIterations; i++)
		{
			obscured = dummy;
		}

		sw.Reset();

		sw.Start();
		for (int i = 0; i < floatIterations; i++)
		{
			dummy = notObscured;
		}

		for (int i = 0; i < floatIterations; i++)
		{
			notObscured = dummy;
		}

		sw.Stop();

		if (dummy != 0) { }
		if (obscured != 0) { }
		if (notObscured != 0) { }
	}

	private void TestInt()
	{

		Stopwatch sw = Stopwatch.StartNew();
		ObscuredInt obscured = 100;
		int notObscured = obscured;
		int dummy = 0;

		for (int i = 0; i < intIterations; i++)
		{
			dummy = obscured;
		}

		for (int i = 0; i < intIterations; i++)
		{
			obscured = dummy;
		}

		sw.Reset();

		sw.Start();
		for (int i = 0; i < intIterations; i++)
		{
			dummy = notObscured;
		}

		for (int i = 0; i < intIterations; i++)
		{
			notObscured = dummy;
		}

		sw.Stop();

		if (dummy != 0) { }
		if (obscured != 0) { }
		if (notObscured != 0) { }
	}

	private void TestLong()
	{

		Stopwatch sw = Stopwatch.StartNew();
		ObscuredLong obscured = 100L;
		long notObscured = obscured;
		long dummy = 0;

		for (int i = 0; i < longIterations; i++)
		{
			dummy = obscured;
		}

		for (int i = 0; i < longIterations; i++)
		{
			obscured = dummy;
		}

		sw.Reset();

		sw.Start();
		for (int i = 0; i < longIterations; i++)
		{
			dummy = notObscured;
		}

		for (int i = 0; i < longIterations; i++)
		{
			notObscured = dummy;
		}

		sw.Stop();

		if (dummy != 0) { }
		if (obscured != 0) { }
		if (notObscured != 0) { }
	}

	private void TestString()
	{

		Stopwatch sw = Stopwatch.StartNew();
		ObscuredString obscured = "abcd";
		string notObscured = obscured;
		string dummy = "";

		for (int i = 0; i < stringIterations; i++)
		{
			dummy = obscured;
		}

		for (int i = 0; i < stringIterations; i++)
		{
			obscured = dummy;
		}

		sw.Reset();

		sw.Start();
		for (int i = 0; i < stringIterations; i++)
		{
			dummy = notObscured;
		}

		for (int i = 0; i < stringIterations; i++)
		{
			notObscured = dummy;
		}

		sw.Stop();

		if (dummy != "") { }
		if (obscured != "") { }
		if (notObscured != "") { }
	}

	private void TestUInt()
	{

		Stopwatch sw = Stopwatch.StartNew();
		ObscuredUInt obscured = 100u;
		uint notObscured = obscured;
		uint dummy = 0;

		for (int i = 0; i < uintIterations; i++)
		{
			dummy = obscured;
		}

		for (int i = 0; i < uintIterations; i++)
		{
			obscured = dummy;
		}

		sw.Reset();

		sw.Start();
		for (int i = 0; i < uintIterations; i++)
		{
			dummy = notObscured;
		}

		for (int i = 0; i < uintIterations; i++)
		{
			notObscured = dummy;
		}

		sw.Stop();

		if (dummy != 0) { }
		if (obscured != 0) { }
		if (notObscured != 0) { }
	}

	private void TestVector3()
	{

		Stopwatch sw = Stopwatch.StartNew();
		ObscuredVector3 obscured = new Vector3(1f, 2f, 3f);
		Vector3 notObscured = obscured;
		Vector3 dummy = new Vector3(0, 0, 0);

		for (int i = 0; i < vector3Iterations; i++)
		{
			dummy = obscured;
		}

		for (int i = 0; i < vector3Iterations; i++)
		{
			obscured = dummy;
		}

		sw.Reset();

		sw.Start();
		for (int i = 0; i < vector3Iterations; i++)
		{
			dummy = notObscured;
		}

		for (int i = 0; i < vector3Iterations; i++)
		{
			notObscured = dummy;
		}

		sw.Stop();

		if (dummy != Vector3.zero) { }
		if (obscured != Vector3.zero) { }
		if (notObscured != Vector3.zero) { }
	}

	private void TestPrefs()
	{

		Stopwatch sw = Stopwatch.StartNew();

		for (int i = 0; i < prefsIterations; i++)
		{
			ObscuredPrefs.SetInt("__a",1);
			ObscuredPrefs.SetFloat("__b",2f);
			ObscuredPrefs.SetString("__c","3");
		}

		for (int i = 0; i < prefsIterations; i++)
		{
			ObscuredPrefs.GetInt("__a", 1);
			ObscuredPrefs.GetFloat("__b", 2f);
			ObscuredPrefs.GetString("__c", "3");
		}

		sw.Reset();

		sw.Start();
		for (int i = 0; i < prefsIterations; i++)
		{
			PlayerPrefs.SetInt("__a", 1);
			PlayerPrefs.SetFloat("__b", 2f);
			PlayerPrefs.SetString("__c", "3");
		}

		for (int i = 0; i < prefsIterations; i++)
		{
			PlayerPrefs.GetInt("__a", 1);
			PlayerPrefs.GetFloat("__b", 2f);
			PlayerPrefs.GetString("__c", "3");
		}

		sw.Stop();

		PlayerPrefs.DeleteKey("__a");
		PlayerPrefs.DeleteKey("__b");
		PlayerPrefs.DeleteKey("__c");
	}
}