using UnityEngine;

public struct Globals {
    public struct Debug {
        public const bool DO_DEBUG = false;
    }

    public struct SceneChangeLocations {
        public struct ForestToArctic {
            public const int X = 191;
            public const int Y = 67;
        }

        public struct ArcticToForest {
            public const int X = 0;
            public const int Y = 45;
        }
    }

    public struct SceneEnteranceLocations {
        public struct InitialSpawn {
            public const int X = 83;
            public const int Y = 83;
        }

        public struct ForestFromArctic {
            public const int X = 190;
            public const int Y = 67;
        }

        public struct ArcticFromForest {
            public const int X = 1;
            public const int Y = 45;
        }
    }
}
