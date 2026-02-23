package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class ArenaMatchResultModule implements IModule
   {
      private static var _instance:ArenaMatchResultModule;
      
      public var damageRecieved:int = 0;
      
      public var playerName:String = "";
      
      public var durationInSeconds:int = 0;
      
      public var damageDealt:int = 0;
      
      public var peakDamage:int = 0;
      
      public function ArenaMatchResultModule(param1:String = "", param2:int = 0, param3:int = 0, param4:int = 0, param5:int = 0)
      {
         super();
         this.damageRecieved = param4;
         this.playerName = param1;
         this.durationInSeconds = param2;
         this.damageDealt = param3;
         this.peakDamage = param5;
      }
      
      public static function get instance() : ArenaMatchResultModule
      {
         return _instance || (_instance = new ArenaMatchResultModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return 683;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 18;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.damageRecieved = param1.readInt();
         this.damageRecieved = this.damageRecieved << 10 | this.damageRecieved >>> 22;
         this.playerName = param1.readUTF();
         this.durationInSeconds = param1.readInt();
         this.durationInSeconds = this.durationInSeconds << 7 | this.durationInSeconds >>> 25;
         this.damageDealt = param1.readInt();
         this.damageDealt = this.damageDealt >>> 12 | this.damageDealt << 20;
         this.peakDamage = param1.readInt();
         this.peakDamage = this.peakDamage << 13 | this.peakDamage >>> 19;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(683);
         param1.writeInt(this.damageRecieved >>> 10 | this.damageRecieved << 22);
         param1.writeUTF(this.playerName);
         param1.writeInt(this.durationInSeconds >>> 7 | this.durationInSeconds << 25);
         param1.writeInt(this.damageDealt << 12 | this.damageDealt >>> 20);
         param1.writeInt(this.peakDamage >>> 13 | this.peakDamage << 19);
      }
   }
}

