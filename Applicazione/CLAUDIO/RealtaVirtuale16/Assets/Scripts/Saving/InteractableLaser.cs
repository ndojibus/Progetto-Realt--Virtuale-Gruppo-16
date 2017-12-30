using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableLaser : InteractableObject_Abstract {

	LaserBehaviour m_laser;

	// Use this for initialization
	void Start () {
		base.Start ();

		m_laser = GetComponentInChildren<LaserBehaviour>();
		if (m_laser == null)
			Debug.LogError("Impossible to find LaserBehaviour!");
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	}

	protected override void setActionText() {
		m_actionText.text = "RUOTA";
	}

	protected override void objectControl() {
		if (!m_laser.enabled)
			m_laser.enabled = true;

		if (m_laser.controlled) {
			m_actionText.text = "RITORNA";
			m_actionCanvas.alpha = 1;
			m_activated = true;
		}
	}

	protected override void endingClickActions() {
	}

	protected override void switchControls()
	{
		base.switchControls ();
		m_laser.controlled = !m_laser.controlled;
	}
}
