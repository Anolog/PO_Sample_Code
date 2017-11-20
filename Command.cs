public class Command : MonoBehaviour {
    //Pending implementation of character object
    //Character* m_Character;
    public void MoveTowards(GameObject player, Vector3 targetPosition)
    {
        // FOR AI
    }
    public void Move(GameObject player, Vector2 direction)
    {
        float maxSpeed = CharacterStats.DEFAULT_MOVEMENT_SPEED * player.GetComponent<CharacterStats>().MovementSpeed;
        if (direction.y < -Mathf.Epsilon)
            maxSpeed *= 0.5f;
        // Changed this to new GDD formula
        Vector3 currentPosition = player.GetComponent<Rigidbody>().position; // need to move the rigidbody, not the transform
        float currentMovementSpeed = player.GetComponent<CharacterStats>().CurrentMovementSpeed;
        currentMovementSpeed = Mathf.Lerp(currentMovementSpeed, maxSpeed, CharacterStats.ACCELERATION_SPEED);
        player.GetComponent<Rigidbody>().MovePosition(currentPosition + (direction.x * player.transform.right + direction.y * player.transform.forward) * currentMovementSpeed * Time.deltaTime);
    }
    public void RotateTowards(GameObject player, Quaternion targetRotation)
    {
        // FOR AI
    }
    public void RotateConstSpeedHorizontal(GameObject player, float horizontalRotation)
    {
        Vector3 rot = player.transform.eulerAngles;
        rot.y += horizontalRotation * player.GetComponent<CharacterStats>().AngularSpeed * Time.deltaTime;
        player.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(rot.x, rot.y, rot.z));
    }
    public void RotatePlayerCameraVertical(GameObject player, float verticalRotation)
    {
        Third_Person_Camera cam = player.GetComponent<CharacterStats>().Camera.GetComponent<Third_Person_Camera>();
        float rot = cam.VerticalRotation;
        rot += verticalRotation * player.GetComponent<CharacterStats>().AngularSpeed * Time.deltaTime;
        rot = Mathf.Clamp(rot, -1, 1);
        cam.VerticalRotation = rot;
    }
    public void RotateHorizontal(GameObject player, float horizontalRotation)
    {
        Vector3 rot = player.GetComponent<Rigidbody>().rotation.eulerAngles;
        rot.y -= horizontalRotation;
        // Have to rotate the rigidbody not the transform
        player.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(rot.x, rot.y, rot.z));
    }
    public void RotateCameraVertical(GameObject player, float verticalRotation)
    {
        Third_Person_Camera cam = player.GetComponent<CharacterStats>().Camera.GetComponent<Third_Person_Camera>();
        float rot = cam.VerticalRotation;
        rot -= verticalRotation;
        rot = Mathf.Clamp(rot, -1, 1);
        cam.VerticalRotation = rot;
    }
    public void Jump(GameObject player)
    {
        player.GetComponent<Rigidbody>().AddForce(new Vector3(0, CharacterStats.JUMP_IMPULSE, 0), ForceMode.Impulse);
    }
    public void UseAbility1(GameObject player)
    {
        player.GetComponent<CharacterStats>().Abilities[1].Use();
    }
    public void UseAbility2(GameObject player)
    {
        player.GetComponent<CharacterStats>().Abilities[2].Use();
    }
    public void UseAbility3(GameObject player)
    {
        player.GetComponent<CharacterStats>().Abilities[3].Use();
    }
    public void UseAbility4(GameObject player)
    {
        player.GetComponent<CharacterStats>().Abilities[4].Use();
    }
    public void UseAttackAbility(GameObject player)
    {
        player.GetComponent<CharacterStats>().Abilities[0].Use();
    }
}