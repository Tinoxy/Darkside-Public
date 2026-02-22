package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class WorldBossInfoCommand implements IModule
   {
      private static var _instance:WorldBossInfoCommand;
      
      public var bannerKey:String = "";
      
      public var currTier:int = 0;
      
      public var timeToEventEnd:Number = 0;
      
      public var sloganKey:String = "";
      
      public var maxAttempts:int = 0;
      
      public var currAttempts:int = 0;
      
      public var maxHealth:Number = 0;
      
      public var worldBossName:String = "";
      
      public var currHealth:Number = 0;
      
      public var descriptionKey:String = "";
      
      public function WorldBossInfoCommand(param1:Number = 0, param2:Number = 0, param3:int = 0, param4:Number = 0, param5:int = 0, param6:int = 0, param7:String = "", param8:String = "", param9:String = "", param10:String = "")
      {
         super();
         this.bannerKey = param7;
         this.currTier = param3;
         this.timeToEventEnd = param4;
         this.sloganKey = param8;
         this.maxAttempts = param6;
         this.currAttempts = param5;
         this.maxHealth = param2;
         this.worldBossName = param10;
         this.currHealth = param1;
         this.descriptionKey = param9;
      }
      
      public static function get instance() : WorldBossInfoCommand
      {
         return _instance || (_instance = new WorldBossInfoCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -25903;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 44;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.bannerKey = param1.readUTF();
         this.currTier = param1.readInt();
         this.currTier = this.currTier << 2 | this.currTier >>> 30;
         this.timeToEventEnd = param1.readDouble();
         this.sloganKey = param1.readUTF();
         this.maxAttempts = param1.readInt();
         this.maxAttempts = this.maxAttempts << 13 | this.maxAttempts >>> 19;
         this.currAttempts = param1.readInt();
         this.currAttempts = this.currAttempts >>> 6 | this.currAttempts << 26;
         this.maxHealth = param1.readDouble();
         this.worldBossName = param1.readUTF();
         this.currHealth = param1.readDouble();
         this.descriptionKey = param1.readUTF();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-25903);
         param1.writeUTF(this.bannerKey);
         param1.writeInt(this.currTier >>> 2 | this.currTier << 30);
         param1.writeDouble(this.timeToEventEnd);
         param1.writeUTF(this.sloganKey);
         param1.writeInt(this.maxAttempts >>> 13 | this.maxAttempts << 19);
         param1.writeInt(this.currAttempts << 6 | this.currAttempts >>> 26);
         param1.writeDouble(this.maxHealth);
         param1.writeUTF(this.worldBossName);
         param1.writeDouble(this.currHealth);
         param1.writeUTF(this.descriptionKey);
      }
   }
}

