using System;
using System.Reflection;
using UnityEditor;

namespace Kogane.Internal
{
    /// <summary>
    /// Scene ビューのアイコンを非表示にするエディタ拡張
    /// </summary>
    internal static class SceneViewIconHider
    {
        //================================================================================
        // 関数（static）
        //================================================================================
        /// <summary>
        /// Unity のメニューから Scene ビューのアイコンを非表示にします
        /// </summary>
        [MenuItem( "Kogane/Scene ビューのアイコンを非表示" )]
        private static void HideFromMenu()
        {
            if ( !EditorUtility.DisplayDialog( "", "Scene ビューのアイコンを非表示にしますか？", "はい", "いいえ" ) ) return;

            var annotation  = Type.GetType( "UnityEditor.Annotation, UnityEditor" );
            var classId     = annotation.GetField( "classID" );
            var scriptClass = annotation.GetField( "scriptClass" );

            var annotationUtility = Type.GetType( "UnityEditor.AnnotationUtility, UnityEditor" );
            var getAnnotations    = annotationUtility.GetMethod( "GetAnnotations", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static );
            var setIconEnabled    = annotationUtility.GetMethod( "SetIconEnabled", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static );

            var annotations = getAnnotations.Invoke( null, null ) as Array;

            foreach ( var n in annotations )
            {
                var parameters = new object[]
                {
                    ( int )classId.GetValue( n ),
                    ( string )scriptClass.GetValue( n ),
                    0,
                };

                setIconEnabled.Invoke( null, parameters );
            }

            EditorUtility.DisplayDialog( "", "Scene ビューのアイコンを非表示にしました", "OK" );
        }
    }
}