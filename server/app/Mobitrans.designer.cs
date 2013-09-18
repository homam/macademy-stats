﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace server
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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="Mobitrans")]
	public partial class MobitransDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertML_App_User(ML_App_User instance);
    partial void UpdateML_App_User(ML_App_User instance);
    partial void DeleteML_App_User(ML_App_User instance);
    #endregion
		
		public MobitransDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["MobitransConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public MobitransDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public MobitransDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public MobitransDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public MobitransDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<ML_App_User> ML_App_Users
		{
			get
			{
				return this.GetTable<ML_App_User>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.ML_App_User")]
	public partial class ML_App_User : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _AppUserId;
		
		private System.Nullable<long> _AppId;
		
		private string _UserId;
		
		private string _DataSource;
		
		private string _Data;
		
		private System.Nullable<System.DateTime> _CreationTime;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnAppUserIdChanging(int value);
    partial void OnAppUserIdChanged();
    partial void OnAppIdChanging(System.Nullable<long> value);
    partial void OnAppIdChanged();
    partial void OnUserIdChanging(string value);
    partial void OnUserIdChanged();
    partial void OnDataSourceChanging(string value);
    partial void OnDataSourceChanged();
    partial void OnDataChanging(string value);
    partial void OnDataChanged();
    partial void OnCreationTimeChanging(System.Nullable<System.DateTime> value);
    partial void OnCreationTimeChanged();
    #endregion
		
		public ML_App_User()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AppUserId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int AppUserId
		{
			get
			{
				return this._AppUserId;
			}
			set
			{
				if ((this._AppUserId != value))
				{
					this.OnAppUserIdChanging(value);
					this.SendPropertyChanging();
					this._AppUserId = value;
					this.SendPropertyChanged("AppUserId");
					this.OnAppUserIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AppId", DbType="BigInt")]
		public System.Nullable<long> AppId
		{
			get
			{
				return this._AppId;
			}
			set
			{
				if ((this._AppId != value))
				{
					this.OnAppIdChanging(value);
					this.SendPropertyChanging();
					this._AppId = value;
					this.SendPropertyChanged("AppId");
					this.OnAppIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserId", DbType="NVarChar(50)")]
		public string UserId
		{
			get
			{
				return this._UserId;
			}
			set
			{
				if ((this._UserId != value))
				{
					this.OnUserIdChanging(value);
					this.SendPropertyChanging();
					this._UserId = value;
					this.SendPropertyChanged("UserId");
					this.OnUserIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DataSource", DbType="NVarChar(50)")]
		public string DataSource
		{
			get
			{
				return this._DataSource;
			}
			set
			{
				if ((this._DataSource != value))
				{
					this.OnDataSourceChanging(value);
					this.SendPropertyChanging();
					this._DataSource = value;
					this.SendPropertyChanged("DataSource");
					this.OnDataSourceChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Data", DbType="Text", UpdateCheck=UpdateCheck.Never)]
		public string Data
		{
			get
			{
				return this._Data;
			}
			set
			{
				if ((this._Data != value))
				{
					this.OnDataChanging(value);
					this.SendPropertyChanging();
					this._Data = value;
					this.SendPropertyChanged("Data");
					this.OnDataChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreationTime", DbType="DateTime")]
		public System.Nullable<System.DateTime> CreationTime
		{
			get
			{
				return this._CreationTime;
			}
			set
			{
				if ((this._CreationTime != value))
				{
					this.OnCreationTimeChanging(value);
					this.SendPropertyChanging();
					this._CreationTime = value;
					this.SendPropertyChanged("CreationTime");
					this.OnCreationTimeChanged();
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
}
#pragma warning restore 1591
