using UnityEngine;

namespace Cargo.Control
{
    public class NPC : TrackFollowerBase
    {
        private void OnCollisionEnter(Collision collision)
        {
            //pathfollower base classından çalıştır


            //kinematik olan rigidbody'yi kaldır
            //PathFollower scriptini öldür
            //col olan objenin pozisyonundan benim pozisyonuma yön çiz o yönde kuvvet uygula
        }
    }
}
