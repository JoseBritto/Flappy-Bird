public static class Constants
{

    //Animator constants
    public const string GOING_UP = "GoingUp";

    public const string GOING_DOWN = "GoingDown";


    public const string OBSTACLE_TAG = "Obstacle";

    public const string BEST_SCORE = "best_score";
    public const string POINT_TAG = "Point";

    // Some utils
    public static float GetPipeHeight(float y) => y * 2.4f + 0.7f;
}
