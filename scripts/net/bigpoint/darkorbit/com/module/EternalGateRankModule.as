package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class EternalGateRankModule implements IModule
   {
      private static var _instance:EternalGateRankModule;
      
      public var lastUpdateTime:String = "";
      
      public var waves:int = 0;
      
      public var rank:int = 0;
      
      public var name:String = "";
      
      public function EternalGateRankModule(param1:int = 0, param2:String = "", param3:String = "", param4:int = 0)
      {
         super();
         this.lastUpdateTime = param3;
         this.waves = param4;
         this.rank = param1;
         this.name = param2;
      }
      
      public static function get instance() : EternalGateRankModule
      {
         return _instance || (_instance = new EternalGateRankModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return 19588;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 12;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.lastUpdateTime = param1.readUTF();
         this.waves = param1.readInt();
         this.waves = this.waves >>> 2 | this.waves << 30;
         this.rank = param1.readInt();
         this.rank = this.rank >>> 14 | this.rank << 18;
         this.name = param1.readUTF();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(19588);
         param1.writeUTF(this.lastUpdateTime);
         param1.writeInt(this.waves << 2 | this.waves >>> 30);
         param1.writeInt(this.rank << 14 | this.rank >>> 18);
         param1.writeUTF(this.name);
      }
   }
}

