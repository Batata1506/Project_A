using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QActivation : MonoBehaviour
{
    [Header("EdgeColliders")]
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private EdgeCollider2D Q1;
    [SerializeField] private EdgeCollider2D Q2;
    [SerializeField] private EdgeCollider2D Q3;
    [SerializeField] private EdgeCollider2D Q4;

    private void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.gameObject.tag.Equals("Player"))
            {
                if (Q1.isTrigger == false && Q2.isTrigger == false)
                {
                    Q1.isTrigger = true;
                    Q1.gameObject.layer = LayerMask.NameToLayer("Default");
                    Q2.isTrigger = true;
                    Q2.gameObject.layer = LayerMask.NameToLayer("Default");
                    Q3.isTrigger = false;
                    Q3.gameObject.layer = LayerMask.NameToLayer("Ground");
                    Q4.isTrigger = false;
                    Q4.gameObject.layer = LayerMask.NameToLayer("Ground");

                sprite.sortingLayerName = "Foreground";
            }

                else if (Q3.isTrigger == false && Q4.isTrigger == false)
                {
                    Q1.isTrigger = false;
                    Q1.gameObject.layer = LayerMask.NameToLayer("Ground");
                    Q2.isTrigger = false;
                    Q2.gameObject.layer = LayerMask.NameToLayer("Ground");
                    Q3.isTrigger = true;
                    Q3.gameObject.layer = LayerMask.NameToLayer("Default");
                    Q4.isTrigger = true;
                    Q4.gameObject.layer = LayerMask.NameToLayer("Default");

                sprite.sortingLayerName = "Foreground behind player";
            }
            }
    }
}
