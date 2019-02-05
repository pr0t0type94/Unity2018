using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchBehaviour : StateMachineBehaviour
{
    PlayerController m_PlayerController;
    public float m_StartPctTime;
    public float m_EndPctTime;
    public enum TPunchType
    {
        LEFT_HAND = 0,
        RIGHT_HAND,
        FOOT
    }
    public TPunchType m_PunchType;
    private void Awake()
    {
        GameObject player= GameObject.FindGameObjectWithTag("Player");
        m_PlayerController= player.GetComponent<PlayerController>();
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //...
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bool l_EnableHandPunch = stateInfo.normalizedTime > m_StartPctTime && stateInfo.normalizedTime < m_EndPctTime;
        if (m_PunchType == TPunchType.LEFT_HAND)
            m_PlayerController.EnableLeftHandPunch(l_EnableHandPunch);
        if (m_PunchType == TPunchType.RIGHT_HAND)
            m_PlayerController.EnableRightHandPunch(l_EnableHandPunch);
        if (m_PunchType == TPunchType.FOOT)
            m_PlayerController.EnableKickAttack(l_EnableHandPunch);
    }
}

