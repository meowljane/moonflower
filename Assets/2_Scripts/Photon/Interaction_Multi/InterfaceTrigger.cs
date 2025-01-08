using UnityEngine;

public interface ITriggerEnter
{
    void OnTriggerEnter2D(Collider2D other);
}

public interface ITriggerExit
{
    void OnTriggerExit2D(Collider2D other);
}
