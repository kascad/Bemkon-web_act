﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProfessorTesting.Old_App_Code
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="professor_testing")]
	public partial class TestDataClassesDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertScaleWeight(ScaleWeight instance);
    partial void UpdateScaleWeight(ScaleWeight instance);
    partial void DeleteScaleWeight(ScaleWeight instance);
    partial void InsertTest(Test instance);
    partial void UpdateTest(Test instance);
    partial void DeleteTest(Test instance);
    partial void InsertScale(Scale instance);
    partial void UpdateScale(Scale instance);
    partial void DeleteScale(Scale instance);
    partial void InsertAnswers(Answers instance);
    partial void UpdateAnswers(Answers instance);
    partial void DeleteAnswers(Answers instance);
    partial void InsertQuestions(Questions instance);
    partial void UpdateQuestions(Questions instance);
    partial void DeleteQuestions(Questions instance);
    #endregion
		
		public TestDataClassesDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["professor_testingConnectionString1"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public TestDataClassesDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public TestDataClassesDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public TestDataClassesDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public TestDataClassesDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<ScaleWeight> ScaleWeights
		{
			get
			{
				return this.GetTable<ScaleWeight>();
			}
		}
		
		public System.Data.Linq.Table<Test> Tests
		{
			get
			{
				return this.GetTable<Test>();
			}
		}
		
		public System.Data.Linq.Table<Scale> Scales
		{
			get
			{
				return this.GetTable<Scale>();
			}
		}
		
		public System.Data.Linq.Table<Answers> Answers
		{
			get
			{
				return this.GetTable<Answers>();
			}
		}
		
		public System.Data.Linq.Table<Questions> Questions
		{
			get
			{
				return this.GetTable<Questions>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.ScaleWeights")]
	public partial class ScaleWeight : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ScaleWeightID;
		
		private System.Nullable<int> _AnsID;
		
		private System.Nullable<int> _ScaleID;
		
		private System.Nullable<double> _Weight;
		
		private EntityRef<Scale> _Scale;
		
		private EntityRef<Answers> _Answers;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnScaleWeightIDChanging(int value);
    partial void OnScaleWeightIDChanged();
    partial void OnAnsIDChanging(System.Nullable<int> value);
    partial void OnAnsIDChanged();
    partial void OnScaleIDChanging(System.Nullable<int> value);
    partial void OnScaleIDChanged();
    partial void OnWeightChanging(System.Nullable<double> value);
    partial void OnWeightChanged();
    #endregion
		
		public ScaleWeight()
		{
			this._Scale = default(EntityRef<Scale>);
			this._Answers = default(EntityRef<Answers>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ScaleWeightID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ScaleWeightID
		{
			get
			{
				return this._ScaleWeightID;
			}
			set
			{
				if ((this._ScaleWeightID != value))
				{
					this.OnScaleWeightIDChanging(value);
					this.SendPropertyChanging();
					this._ScaleWeightID = value;
					this.SendPropertyChanged("ScaleWeightID");
					this.OnScaleWeightIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AnsID", DbType="Int")]
		public System.Nullable<int> AnsID
		{
			get
			{
				return this._AnsID;
			}
			set
			{
				if ((this._AnsID != value))
				{
					if (this._Answers.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnAnsIDChanging(value);
					this.SendPropertyChanging();
					this._AnsID = value;
					this.SendPropertyChanged("AnsID");
					this.OnAnsIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ScaleID", DbType="Int")]
		public System.Nullable<int> ScaleID
		{
			get
			{
				return this._ScaleID;
			}
			set
			{
				if ((this._ScaleID != value))
				{
					if (this._Scale.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnScaleIDChanging(value);
					this.SendPropertyChanging();
					this._ScaleID = value;
					this.SendPropertyChanged("ScaleID");
					this.OnScaleIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Weight", DbType="Float")]
		public System.Nullable<double> Weight
		{
			get
			{
				return this._Weight;
			}
			set
			{
				if ((this._Weight != value))
				{
					this.OnWeightChanging(value);
					this.SendPropertyChanging();
					this._Weight = value;
					this.SendPropertyChanged("Weight");
					this.OnWeightChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Scale_ScaleWeight", Storage="_Scale", ThisKey="ScaleID", OtherKey="ScaleID", IsForeignKey=true, DeleteRule="CASCADE")]
		public Scale Scale
		{
			get
			{
				return this._Scale.Entity;
			}
			set
			{
				Scale previousValue = this._Scale.Entity;
				if (((previousValue != value) 
							|| (this._Scale.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Scale.Entity = null;
						previousValue.ScaleWeights.Remove(this);
					}
					this._Scale.Entity = value;
					if ((value != null))
					{
						value.ScaleWeights.Add(this);
						this._ScaleID = value.ScaleID;
					}
					else
					{
						this._ScaleID = default(Nullable<int>);
					}
					this.SendPropertyChanged("Scale");
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Answers_ScaleWeight", Storage="_Answers", ThisKey="AnsID", OtherKey="AnsID", IsForeignKey=true, DeleteRule="CASCADE")]
		public Answers Answers
		{
			get
			{
				return this._Answers.Entity;
			}
			set
			{
				Answers previousValue = this._Answers.Entity;
				if (((previousValue != value) 
							|| (this._Answers.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Answers.Entity = null;
						previousValue.ScaleWeight.Remove(this);
					}
					this._Answers.Entity = value;
					if ((value != null))
					{
						value.ScaleWeight.Add(this);
						this._AnsID = value.AnsID;
					}
					else
					{
						this._AnsID = default(Nullable<int>);
					}
					this.SendPropertyChanged("Answers");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Tests")]
	public partial class Test : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _TestID;
		
		private string _ShortName;
		
		private string _FullName;
		
		private int _CategoryID;
		
		private string _Author;
		
		private System.Nullable<System.DateTime> _Date;
		
		private string _Description;
		
		private int _TestingCount;
		
		private string _Preamble;
		
		private EntitySet<Scale> _Scales;
		
		private EntitySet<Questions> _Questions;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnTestIDChanging(int value);
    partial void OnTestIDChanged();
    partial void OnShortNameChanging(string value);
    partial void OnShortNameChanged();
    partial void OnFullNameChanging(string value);
    partial void OnFullNameChanged();
    partial void OnCategoryIDChanging(int value);
    partial void OnCategoryIDChanged();
    partial void OnAuthorChanging(string value);
    partial void OnAuthorChanged();
    partial void OnDateChanging(System.Nullable<System.DateTime> value);
    partial void OnDateChanged();
    partial void OnDescriptionChanging(string value);
    partial void OnDescriptionChanged();
    partial void OnTestingCountChanging(int value);
    partial void OnTestingCountChanged();
    partial void OnPreambleChanging(string value);
    partial void OnPreambleChanged();
    #endregion
		
		public Test()
		{
			this._Scales = new EntitySet<Scale>(new Action<Scale>(this.attach_Scales), new Action<Scale>(this.detach_Scales));
			this._Questions = new EntitySet<Questions>(new Action<Questions>(this.attach_Questions), new Action<Questions>(this.detach_Questions));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TestID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int TestID
		{
			get
			{
				return this._TestID;
			}
			set
			{
				if ((this._TestID != value))
				{
					this.OnTestIDChanging(value);
					this.SendPropertyChanging();
					this._TestID = value;
					this.SendPropertyChanged("TestID");
					this.OnTestIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ShortName", DbType="NVarChar(5) NOT NULL", CanBeNull=false)]
		public string ShortName
		{
			get
			{
				return this._ShortName;
			}
			set
			{
				if ((this._ShortName != value))
				{
					this.OnShortNameChanging(value);
					this.SendPropertyChanging();
					this._ShortName = value;
					this.SendPropertyChanged("ShortName");
					this.OnShortNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FullName", DbType="NVarChar(100) NOT NULL", CanBeNull=false)]
		public string FullName
		{
			get
			{
				return this._FullName;
			}
			set
			{
				if ((this._FullName != value))
				{
					this.OnFullNameChanging(value);
					this.SendPropertyChanging();
					this._FullName = value;
					this.SendPropertyChanged("FullName");
					this.OnFullNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CategoryID", DbType="Int NOT NULL")]
		public int CategoryID
		{
			get
			{
				return this._CategoryID;
			}
			set
			{
				if ((this._CategoryID != value))
				{
					this.OnCategoryIDChanging(value);
					this.SendPropertyChanging();
					this._CategoryID = value;
					this.SendPropertyChanged("CategoryID");
					this.OnCategoryIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Author", DbType="NVarChar(50)")]
		public string Author
		{
			get
			{
				return this._Author;
			}
			set
			{
				if ((this._Author != value))
				{
					this.OnAuthorChanging(value);
					this.SendPropertyChanging();
					this._Author = value;
					this.SendPropertyChanged("Author");
					this.OnAuthorChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Date", DbType="DateTime")]
		public System.Nullable<System.DateTime> Date
		{
			get
			{
				return this._Date;
			}
			set
			{
				if ((this._Date != value))
				{
					this.OnDateChanging(value);
					this.SendPropertyChanging();
					this._Date = value;
					this.SendPropertyChanged("Date");
					this.OnDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Description", DbType="NVarChar(150)")]
		public string Description
		{
			get
			{
				return this._Description;
			}
			set
			{
				if ((this._Description != value))
				{
					this.OnDescriptionChanging(value);
					this.SendPropertyChanging();
					this._Description = value;
					this.SendPropertyChanged("Description");
					this.OnDescriptionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TestingCount", DbType="Int NOT NULL")]
		public int TestingCount
		{
			get
			{
				return this._TestingCount;
			}
			set
			{
				if ((this._TestingCount != value))
				{
					this.OnTestingCountChanging(value);
					this.SendPropertyChanging();
					this._TestingCount = value;
					this.SendPropertyChanged("TestingCount");
					this.OnTestingCountChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Preamble", DbType="NVarChar(50)")]
		public string Preamble
		{
			get
			{
				return this._Preamble;
			}
			set
			{
				if ((this._Preamble != value))
				{
					this.OnPreambleChanging(value);
					this.SendPropertyChanging();
					this._Preamble = value;
					this.SendPropertyChanged("Preamble");
					this.OnPreambleChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Test_Scale", Storage="_Scales", ThisKey="TestID", OtherKey="TestID")]
		public EntitySet<Scale> Scales
		{
			get
			{
				return this._Scales;
			}
			set
			{
				this._Scales.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Test_Questions", Storage="_Questions", ThisKey="TestID", OtherKey="TestID")]
		public EntitySet<Questions> Questions
		{
			get
			{
				return this._Questions;
			}
			set
			{
				this._Questions.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_Scales(Scale entity)
		{
			this.SendPropertyChanging();
			entity.Test = this;
		}
		
		private void detach_Scales(Scale entity)
		{
			this.SendPropertyChanging();
			entity.Test = null;
		}
		
		private void attach_Questions(Questions entity)
		{
			this.SendPropertyChanging();
			entity.Test = this;
		}
		
		private void detach_Questions(Questions entity)
		{
			this.SendPropertyChanging();
			entity.Test = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Scales")]
	public partial class Scale : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ScaleID;
		
		private string _ScaleName;
		
		private string _ScaleShortName;
		
		private int _TestID;
		
		private System.Nullable<double> _BallAVR;
		
		private System.Nullable<double> _BallMin;
		
		private System.Nullable<double> _BallMax;
		
		private System.Nullable<double> _BallSTD;
		
		private EntitySet<ScaleWeight> _ScaleWeights;
		
		private EntityRef<Test> _Test;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnScaleIDChanging(int value);
    partial void OnScaleIDChanged();
    partial void OnScaleNameChanging(string value);
    partial void OnScaleNameChanged();
    partial void OnScaleShortNameChanging(string value);
    partial void OnScaleShortNameChanged();
    partial void OnTestIDChanging(int value);
    partial void OnTestIDChanged();
    partial void OnBallAVRChanging(System.Nullable<double> value);
    partial void OnBallAVRChanged();
    partial void OnBallMinChanging(System.Nullable<double> value);
    partial void OnBallMinChanged();
    partial void OnBallMaxChanging(System.Nullable<double> value);
    partial void OnBallMaxChanged();
    partial void OnBallSTDChanging(System.Nullable<double> value);
    partial void OnBallSTDChanged();
    #endregion
		
		public Scale()
		{
			this._ScaleWeights = new EntitySet<ScaleWeight>(new Action<ScaleWeight>(this.attach_ScaleWeights), new Action<ScaleWeight>(this.detach_ScaleWeights));
			this._Test = default(EntityRef<Test>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ScaleID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ScaleID
		{
			get
			{
				return this._ScaleID;
			}
			set
			{
				if ((this._ScaleID != value))
				{
					this.OnScaleIDChanging(value);
					this.SendPropertyChanging();
					this._ScaleID = value;
					this.SendPropertyChanged("ScaleID");
					this.OnScaleIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ScaleName", DbType="NVarChar(20)")]
		public string ScaleName
		{
			get
			{
				return this._ScaleName;
			}
			set
			{
				if ((this._ScaleName != value))
				{
					this.OnScaleNameChanging(value);
					this.SendPropertyChanging();
					this._ScaleName = value;
					this.SendPropertyChanged("ScaleName");
					this.OnScaleNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ScaleShortName", DbType="NVarChar(2)")]
		public string ScaleShortName
		{
			get
			{
				return this._ScaleShortName;
			}
			set
			{
				if ((this._ScaleShortName != value))
				{
					this.OnScaleShortNameChanging(value);
					this.SendPropertyChanging();
					this._ScaleShortName = value;
					this.SendPropertyChanged("ScaleShortName");
					this.OnScaleShortNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TestID", DbType="Int NOT NULL")]
		public int TestID
		{
			get
			{
				return this._TestID;
			}
			set
			{
				if ((this._TestID != value))
				{
					if (this._Test.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnTestIDChanging(value);
					this.SendPropertyChanging();
					this._TestID = value;
					this.SendPropertyChanged("TestID");
					this.OnTestIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BallAVR", DbType="Float")]
		public System.Nullable<double> BallAVR
		{
			get
			{
				return this._BallAVR;
			}
			set
			{
				if ((this._BallAVR != value))
				{
					this.OnBallAVRChanging(value);
					this.SendPropertyChanging();
					this._BallAVR = value;
					this.SendPropertyChanged("BallAVR");
					this.OnBallAVRChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BallMin", DbType="Float")]
		public System.Nullable<double> BallMin
		{
			get
			{
				return this._BallMin;
			}
			set
			{
				if ((this._BallMin != value))
				{
					this.OnBallMinChanging(value);
					this.SendPropertyChanging();
					this._BallMin = value;
					this.SendPropertyChanged("BallMin");
					this.OnBallMinChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BallMax", DbType="Float")]
		public System.Nullable<double> BallMax
		{
			get
			{
				return this._BallMax;
			}
			set
			{
				if ((this._BallMax != value))
				{
					this.OnBallMaxChanging(value);
					this.SendPropertyChanging();
					this._BallMax = value;
					this.SendPropertyChanged("BallMax");
					this.OnBallMaxChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BallSTD", DbType="Float")]
		public System.Nullable<double> BallSTD
		{
			get
			{
				return this._BallSTD;
			}
			set
			{
				if ((this._BallSTD != value))
				{
					this.OnBallSTDChanging(value);
					this.SendPropertyChanging();
					this._BallSTD = value;
					this.SendPropertyChanged("BallSTD");
					this.OnBallSTDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Scale_ScaleWeight", Storage="_ScaleWeights", ThisKey="ScaleID", OtherKey="ScaleID")]
		public EntitySet<ScaleWeight> ScaleWeights
		{
			get
			{
				return this._ScaleWeights;
			}
			set
			{
				this._ScaleWeights.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Test_Scale", Storage="_Test", ThisKey="TestID", OtherKey="TestID", IsForeignKey=true)]
		public Test Test
		{
			get
			{
				return this._Test.Entity;
			}
			set
			{
				Test previousValue = this._Test.Entity;
				if (((previousValue != value) 
							|| (this._Test.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Test.Entity = null;
						previousValue.Scales.Remove(this);
					}
					this._Test.Entity = value;
					if ((value != null))
					{
						value.Scales.Add(this);
						this._TestID = value.TestID;
					}
					else
					{
						this._TestID = default(int);
					}
					this.SendPropertyChanged("Test");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_ScaleWeights(ScaleWeight entity)
		{
			this.SendPropertyChanging();
			entity.Scale = this;
		}
		
		private void detach_ScaleWeights(ScaleWeight entity)
		{
			this.SendPropertyChanging();
			entity.Scale = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Answers")]
	public partial class Answers : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _AnsID;
		
		private int _QuestID;
		
		private System.Nullable<int> _AnsNum;
		
		private string _AnsText;
		
		private System.Nullable<int> _NextQuestNum;
		
		private EntitySet<ScaleWeight> _ScaleWeight;
		
		private EntityRef<Questions> _Questions;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnAnsIDChanging(int value);
    partial void OnAnsIDChanged();
    partial void OnQuestIDChanging(int value);
    partial void OnQuestIDChanged();
    partial void OnAnsNumChanging(System.Nullable<int> value);
    partial void OnAnsNumChanged();
    partial void OnAnsTextChanging(string value);
    partial void OnAnsTextChanged();
    partial void OnNextQuestNumChanging(System.Nullable<int> value);
    partial void OnNextQuestNumChanged();
    #endregion
		
		public Answers()
		{
			this._ScaleWeight = new EntitySet<ScaleWeight>(new Action<ScaleWeight>(this.attach_ScaleWeight), new Action<ScaleWeight>(this.detach_ScaleWeight));
			this._Questions = default(EntityRef<Questions>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AnsID", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int AnsID
		{
			get
			{
				return this._AnsID;
			}
			set
			{
				if ((this._AnsID != value))
				{
					this.OnAnsIDChanging(value);
					this.SendPropertyChanging();
					this._AnsID = value;
					this.SendPropertyChanged("AnsID");
					this.OnAnsIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_QuestID", DbType="Int NOT NULL")]
		public int QuestID
		{
			get
			{
				return this._QuestID;
			}
			set
			{
				if ((this._QuestID != value))
				{
					if (this._Questions.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnQuestIDChanging(value);
					this.SendPropertyChanging();
					this._QuestID = value;
					this.SendPropertyChanged("QuestID");
					this.OnQuestIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AnsNum", DbType="Int")]
		public System.Nullable<int> AnsNum
		{
			get
			{
				return this._AnsNum;
			}
			set
			{
				if ((this._AnsNum != value))
				{
					this.OnAnsNumChanging(value);
					this.SendPropertyChanging();
					this._AnsNum = value;
					this.SendPropertyChanged("AnsNum");
					this.OnAnsNumChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AnsText", DbType="NVarChar(150) NOT NULL", CanBeNull=false)]
		public string AnsText
		{
			get
			{
				return this._AnsText;
			}
			set
			{
				if ((this._AnsText != value))
				{
					this.OnAnsTextChanging(value);
					this.SendPropertyChanging();
					this._AnsText = value;
					this.SendPropertyChanged("AnsText");
					this.OnAnsTextChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NextQuestNum", DbType="Int")]
		public System.Nullable<int> NextQuestNum
		{
			get
			{
				return this._NextQuestNum;
			}
			set
			{
				if ((this._NextQuestNum != value))
				{
					this.OnNextQuestNumChanging(value);
					this.SendPropertyChanging();
					this._NextQuestNum = value;
					this.SendPropertyChanged("NextQuestNum");
					this.OnNextQuestNumChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Answers_ScaleWeight", Storage="_ScaleWeight", ThisKey="AnsID", OtherKey="AnsID")]
		public EntitySet<ScaleWeight> ScaleWeight
		{
			get
			{
				return this._ScaleWeight;
			}
			set
			{
				this._ScaleWeight.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Questions_Answers", Storage="_Questions", ThisKey="QuestID", OtherKey="QuestID", IsForeignKey=true, DeleteOnNull=true, DeleteRule="CASCADE")]
		public Questions Questions
		{
			get
			{
				return this._Questions.Entity;
			}
			set
			{
				Questions previousValue = this._Questions.Entity;
				if (((previousValue != value) 
							|| (this._Questions.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Questions.Entity = null;
						previousValue.Answers.Remove(this);
					}
					this._Questions.Entity = value;
					if ((value != null))
					{
						value.Answers.Add(this);
						this._QuestID = value.QuestID;
					}
					else
					{
						this._QuestID = default(int);
					}
					this.SendPropertyChanged("Questions");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_ScaleWeight(ScaleWeight entity)
		{
			this.SendPropertyChanging();
			entity.Answers = this;
		}
		
		private void detach_ScaleWeight(ScaleWeight entity)
		{
			this.SendPropertyChanging();
			entity.Answers = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Questions")]
	public partial class Questions : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _QuestID;
		
		private int _TestID;
		
		private string _QuestText;
		
		private int _QuestType;
		
		private System.Nullable<int> _QuestNum;
		
		private System.Data.Linq.Binary _QuestImg;
		
		private EntitySet<Answers> _Answers;
		
		private EntityRef<Test> _Test;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnQuestIDChanging(int value);
    partial void OnQuestIDChanged();
    partial void OnTestIDChanging(int value);
    partial void OnTestIDChanged();
    partial void OnQuestTextChanging(string value);
    partial void OnQuestTextChanged();
    partial void OnQuestTypeChanging(int value);
    partial void OnQuestTypeChanged();
    partial void OnQuestNumChanging(System.Nullable<int> value);
    partial void OnQuestNumChanged();
    partial void OnQuestImgChanging(System.Data.Linq.Binary value);
    partial void OnQuestImgChanged();
    #endregion
		
		public Questions()
		{
			this._Answers = new EntitySet<Answers>(new Action<Answers>(this.attach_Answers), new Action<Answers>(this.detach_Answers));
			this._Test = default(EntityRef<Test>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_QuestID", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int QuestID
		{
			get
			{
				return this._QuestID;
			}
			set
			{
				if ((this._QuestID != value))
				{
					this.OnQuestIDChanging(value);
					this.SendPropertyChanging();
					this._QuestID = value;
					this.SendPropertyChanged("QuestID");
					this.OnQuestIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TestID", DbType="Int NOT NULL")]
		public int TestID
		{
			get
			{
				return this._TestID;
			}
			set
			{
				if ((this._TestID != value))
				{
					if (this._Test.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnTestIDChanging(value);
					this.SendPropertyChanging();
					this._TestID = value;
					this.SendPropertyChanged("TestID");
					this.OnTestIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_QuestText", DbType="NVarChar(240) NOT NULL", CanBeNull=false)]
		public string QuestText
		{
			get
			{
				return this._QuestText;
			}
			set
			{
				if ((this._QuestText != value))
				{
					this.OnQuestTextChanging(value);
					this.SendPropertyChanging();
					this._QuestText = value;
					this.SendPropertyChanged("QuestText");
					this.OnQuestTextChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_QuestType", DbType="Int NOT NULL")]
		public int QuestType
		{
			get
			{
				return this._QuestType;
			}
			set
			{
				if ((this._QuestType != value))
				{
					this.OnQuestTypeChanging(value);
					this.SendPropertyChanging();
					this._QuestType = value;
					this.SendPropertyChanged("QuestType");
					this.OnQuestTypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_QuestNum", DbType="Int")]
		public System.Nullable<int> QuestNum
		{
			get
			{
				return this._QuestNum;
			}
			set
			{
				if ((this._QuestNum != value))
				{
					this.OnQuestNumChanging(value);
					this.SendPropertyChanging();
					this._QuestNum = value;
					this.SendPropertyChanged("QuestNum");
					this.OnQuestNumChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_QuestImg", DbType="VarBinary(MAX)", UpdateCheck=UpdateCheck.Never)]
		public System.Data.Linq.Binary QuestImg
		{
			get
			{
				return this._QuestImg;
			}
			set
			{
				if ((this._QuestImg != value))
				{
					this.OnQuestImgChanging(value);
					this.SendPropertyChanging();
					this._QuestImg = value;
					this.SendPropertyChanged("QuestImg");
					this.OnQuestImgChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Questions_Answers", Storage="_Answers", ThisKey="QuestID", OtherKey="QuestID")]
		public EntitySet<Answers> Answers
		{
			get
			{
				return this._Answers;
			}
			set
			{
				this._Answers.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Test_Questions", Storage="_Test", ThisKey="TestID", OtherKey="TestID", IsForeignKey=true, DeleteOnNull=true, DeleteRule="CASCADE")]
		public Test Test
		{
			get
			{
				return this._Test.Entity;
			}
			set
			{
				Test previousValue = this._Test.Entity;
				if (((previousValue != value) 
							|| (this._Test.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Test.Entity = null;
						previousValue.Questions.Remove(this);
					}
					this._Test.Entity = value;
					if ((value != null))
					{
						value.Questions.Add(this);
						this._TestID = value.TestID;
					}
					else
					{
						this._TestID = default(int);
					}
					this.SendPropertyChanged("Test");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_Answers(Answers entity)
		{
			this.SendPropertyChanging();
			entity.Questions = this;
		}
		
		private void detach_Answers(Answers entity)
		{
			this.SendPropertyChanging();
			entity.Questions = null;
		}
	}
}
#pragma warning restore 1591