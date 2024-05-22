using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using PG_Physics.Wheel;
using UnityEngine.SceneManagement;
using System.Runtime.ConstrainedExecution;

/// <summary>
/// Wheel settings and update logic.
/// </summary>
[System.Serializable]
public struct Wheel
{
	public WheelCollider WheelCollider;
	public Text scoreText;
    public Transform WheelView;
	public float SlipForGenerateParticle;
	public Vector3 TrailOffset;
	public bool isDrifting;
	public int score;
	public int goalScore;
	public GameObject win;
	public GameObject timer;
	public bool won;


    public float CurrentMaxSlip { get { return Mathf.Max (CurrentForwardSleep, CurrentSidewaysSleep); } }
	public float CurrentForwardSleep { get; private set; }
	public float CurrentSidewaysSleep { get; private set; }
	public WheelHit GetHit { get { return Hit; } }



    WheelHit Hit;
	TrailRenderer Trail;

	PG_WheelCollider m_PGWC;
	public PG_WheelCollider PG_WheelCollider
	{
		get
		{
			if (m_PGWC == null)
			{
				m_PGWC = WheelCollider.GetComponent<PG_WheelCollider> ();
			}
			if (m_PGWC == null)
			{
				m_PGWC = WheelCollider.gameObject.AddComponent<PG_WheelCollider> ();
				m_PGWC.CheckFirstEnable ();
			}
			return m_PGWC;
		}
	}
    public void Start()
	{
		score = 0;
		won = false;

    }
    FXController FXController { get { return FXController.Instance; } }
	Vector3 HitPoint;

	const int SmoothValuesCount = 3;

	public void FixedUpdate ()
	{

		if (WheelCollider.GetGroundHit (out Hit))
		{
			var prevForwar = CurrentForwardSleep;
			var prevSide = CurrentSidewaysSleep;

			CurrentForwardSleep = (prevForwar + Mathf.Abs (Hit.forwardSlip)) / 2;
			CurrentSidewaysSleep = (prevSide + Mathf.Abs (Hit.sidewaysSlip)) / 2;
		}
		else
		{
			CurrentForwardSleep = 0;
			CurrentSidewaysSleep = 0;
		}
		if (isDrifting == true)
		{
			score += 1;

        }
		scoreText.text = score.ToString ();

		if (score >= goalScore && won == false)
		{
			win.gameObject.SetActive(true);
            timer.GetComponent<Timer>().ExitMainMenuT();
            won =true;

        }
	}

	/// <summary>
	/// Update visual logic (Transform, FX).
	/// </summary>
	public void UpdateVisual ()
	{
		UpdateTransform ();

		if (WheelCollider.isGrounded && CurrentMaxSlip > SlipForGenerateParticle)
		{
			//Emit particle.
			var particles = FXController.GetAspahaltParticles;
			var point = WheelCollider.transform.position;
			point.y = Hit.point.y;
			particles.transform.position = point;
			particles.Emit (1);
			isDrifting = true;

			if (Trail == null)
			{
				//Get free or create trail.
				HitPoint = WheelCollider.transform.position;
				HitPoint.y = Hit.point.y;
				Trail = FXController.GetTrail (HitPoint);
				Trail.transform.SetParent (WheelCollider.transform);
				Trail.transform.localPosition += TrailOffset;
			}
		}
		else if (Trail != null)
		{
			//Set trail as free.
			FXController.SetFreeTrail (Trail);
			Trail = null;
            isDrifting = false;
        }
		else{
            isDrifting = false;
        }
	}

	public void UpdateTransform ()
	{
		Vector3 pos;
		Quaternion quat;
		WheelCollider.GetWorldPose (out pos, out quat);
		WheelView.position = pos;
		WheelView.rotation = quat;
	}

	public void UpdateFrictionConfig (PG_WheelColliderConfig config)
	{
		PG_WheelCollider.UpdateConfig (config);
	}
}
