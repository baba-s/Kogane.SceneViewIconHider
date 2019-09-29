using System;
using System.Reflection;
using UnityEditor;

namespace KoganeUnityLib
{
	public static class SceneViewIconHider
	{
		[MenuItem( "Edit/Hide Scene View Icon" )]
		private static void Hide()
		{
			const BindingFlags bindingAttrs =
				BindingFlags.NonPublic | 
				BindingFlags.Public | 
				BindingFlags.Static;

			var annotation  = Type.GetType( "UnityEditor.Annotation, UnityEditor" );
			var classId     = annotation.GetField( "classID" );
			var scriptClass = annotation.GetField( "scriptClass" );

			var annotationUtility = Type.GetType( "UnityEditor.AnnotationUtility, UnityEditor" );
			var getAnnotations    = annotationUtility.GetMethod( "GetAnnotations", bindingAttrs );
			var setIconEnabled    = annotationUtility.GetMethod( "SetIconEnabled", bindingAttrs );

			var annotations = getAnnotations.Invoke( null, null ) as Array;

			foreach ( var n in annotations )
			{
				var parameters = new object[]
				{
					( int ) classId.GetValue( n ),
					( string ) scriptClass.GetValue( n ),
					0,
				};

				setIconEnabled.Invoke( null, parameters );
			}
		}
	}
}