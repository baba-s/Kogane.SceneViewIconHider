using System;
using System.Reflection;
using UnityEditor;

namespace UniSceneViewIconHider
{
	/// <summary>
	/// Scene ビューのアイコンを非表示にするエディタ拡張
	/// </summary>
	public static class SceneViewIconHider
	{
		//================================================================================
		// デリゲート（static）
		//================================================================================
		public static Action OnHide { private get; set; }

		//================================================================================
		// 関数（static）
		//================================================================================
		/// <summary>
		/// Unity のメニューから Scene ビューのアイコンを非表示にします
		/// </summary>
		[MenuItem( "Edit/UniSceneViewIconHider/Scene ビューのアイコンを非表示" )]
		private static void HideFromMenu()
		{
			if ( OnHide != null )
			{
				OnHide();
				return;
			}

			Hide();
		}

		/// <summary>
		/// Scene ビューのアイコンを非表示にします
		/// </summary>
		public static void Hide()
		{
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
					( int ) classId.GetValue( n ),
					( string ) scriptClass.GetValue( n ),
					0,
				};

				setIconEnabled.Invoke( null, parameters );
			}
		}
	}
}