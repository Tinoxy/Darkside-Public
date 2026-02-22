package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   
   public class AssemblyRecipeBuildStatusModule extends AssemblyRecipeStatusModule
   {
      private static var _instance:AssemblyRecipeBuildStatusModule;
      
      public var remainingTimeMillis:Number = 0;
      
      public var buildAmount:Number = 0;
      
      public var prevBuildCount:int = 0;
      
      public function AssemblyRecipeBuildStatusModule(param1:Number = 0, param2:Number = 0, param3:int = 0)
      {
         super();
         this.remainingTimeMillis = param1;
         this.buildAmount = param2;
         this.prevBuildCount = param3;
      }
      
      public static function get instance() : AssemblyRecipeBuildStatusModule
      {
         return _instance || (_instance = new AssemblyRecipeBuildStatusModule());
      }
      
      override public function getLibcomModuleId() : int
      {
         return 1844;
      }
      
      override public function getLibcomModuleEstimatedLength() : int
      {
         return 20;
      }
      
      override public function read(param1:IDataInput) : void
      {
         param1.readShort();
         super.read(param1);
         this.remainingTimeMillis = param1.readDouble();
         this.buildAmount = param1.readDouble();
         this.prevBuildCount = param1.readInt();
         this.prevBuildCount = this.prevBuildCount << 9 | this.prevBuildCount >>> 23;
      }
      
      override public function write(param1:IDataOutput) : void
      {
         param1.writeShort(1844);
         super.write(param1);
         param1.writeDouble(this.remainingTimeMillis);
         param1.writeDouble(this.buildAmount);
         param1.writeInt(this.prevBuildCount >>> 9 | this.prevBuildCount << 23);
      }
   }
}

