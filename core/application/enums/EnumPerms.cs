using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia_Docker.core.application.enums
{
    [Flags]
    internal enum EnumPerms : ulong
    {
        None = 0,
        //
        // Resumo:
        //     Allows creation of instant invites.
        CreateInstantInvite = 1uL,
        //
        // Resumo:
        //     Allows kicking guild users.
        KickUsers = 2uL,
        //
        // Resumo:
        //     Allows banning guild users.
        BanUsers = 4uL,
        //
        // Resumo:
        //     Allows all permissions and bypasses channel permission overwrites.
        Administrator = 8uL,
        //
        // Resumo:
        //     Allows management and editing of channels.
        ManageChannels = 0x10uL,
        //
        // Resumo:
        //     Allows management and editing of the guild.
        ManageGuild = 0x20uL,
        //
        // Resumo:
        //     Allows for the addition of reactions to messages.
        AddReactions = 0x40uL,
        //
        // Resumo:
        //     Allows for viewing of audit logs.
        ViewAuditLog = 0x80uL,
        //
        // Resumo:
        //     Allows for using priority speaker in a voice channel.
        PrioritySpeaker = 0x100uL,
        //
        // Resumo:
        //     Allows the user to go live.
        Stream = 0x200uL,
        //
        // Resumo:
        //     Allows guild users to view a channel, which includes reading messages in text
        //     channels and joining voice channels.
        ViewChannel = 0x400uL,
        //
        // Resumo:
        //     Allows for sending messages in a channel and creating threads in a forum (does
        //     not allow sending messages in threads).
        SendMessages = 0x800uL,
        //
        // Resumo:
        //     Allows for sending of /tts messages.
        SendTtsMessages = 0x1000uL,
        //
        // Resumo:
        //     Allows for deletion of other users messages.
        ManageMessages = 0x2000uL,
        //
        // Resumo:
        //     Links sent by users with this permission will be auto-embedded.
        EmbedLinks = 0x4000uL,
        //
        // Resumo:
        //     Allows for uploading images and files.
        AttachFiles = 0x8000uL,
        //
        // Resumo:
        //     Allows for reading of message history.
        ReadMessageHistory = 0x10000uL,
        //
        // Resumo:
        //     Allows for using the @everyone tag to notify all users in a channel, and the
        //     @here tag to notify all online users in a channel.
        MentionEveryone = 0x20000uL,
        //
        // Resumo:
        //     Allows the usage of custom emojis from other servers.
        UseExternalEmojis = 0x40000uL,
        //
        // Resumo:
        //     Allows for viewing guild insights.
        ViewGuildInsights = 0x80000uL,
        //
        // Resumo:
        //     Allows for joining of a voice channel.
        Connect = 0x100000uL,
        //
        // Resumo:
        //     Allows for speaking in a voice channel.
        Speak = 0x200000uL,
        //
        // Resumo:
        //     Allows for muting users in a voice channel.
        MuteUsers = 0x400000uL,
        //
        // Resumo:
        //     Allows for deafening of users in a voice channel.
        DeafenUsers = 0x800000uL,
        //
        // Resumo:
        //     Allows for moving of users between voice channels.
        MoveUsers = 0x1000000uL,
        //
        // Resumo:
        //     Allows for using voice-activity-detection in a voice channel.
        UseVoiceActivityDetection = 0x2000000uL,
        //
        // Resumo:
        //     Allows for modification of own nickname.
        ChangeNickname = 0x4000000uL,
        //
        // Resumo:
        //     Allows for modification of other users nicknames.
        ManageNicknames = 0x8000000uL,
        //
        // Resumo:
        //     Allows management and editing of roles.
        ManageRoles = 0x10000000uL,
        //
        // Resumo:
        //     Allows management and editing of webhooks.
        ManageWebhooks = 0x20000000uL,
        //
        // Resumo:
        //     Allows for editing and deleting emojis, stickers, and soundboard sounds created
        //     by all users.
        ManageGuildExpressions = 0x40000000uL,
        //
        // Resumo:
        //     Allows users to use application commands, including slash commands and context
        //     menu commands.
        UseApplicationCommands = 0x80000000uL,
        //
        // Resumo:
        //     Allows for requesting to speak in stage channels.
        RequestToSpeak = 0x100000000uL,
        //
        // Resumo:
        //     Allows for creating, editing and deleting scheduled events created by all users.
        ManageEvents = 0x200000000uL,
        //
        // Resumo:
        //     Allows for deleting and archiving threads, and viewing all private threads.
        ManageThreads = 0x400000000uL,
        //
        // Resumo:
        //     Allows for creating public and announcement threads.
        CreatePublicThreads = 0x800000000uL,
        //
        // Resumo:
        //     Allows for creating private threads.
        CreatePrivateThreads = 0x1000000000uL,
        //
        // Resumo:
        //     Allows the usage of custom stickers from other servers.
        UseExternalStickers = 0x2000000000uL,
        //
        // Resumo:
        //     Allows for sending messages in threads.
        SendMessagesInThreads = 0x4000000000uL,
        //
        // Resumo:
        //     Allows for using Activities (applications with the NetCord.ApplicationFlags.Embedded
        //     flag) in a voice channel.
        StartEmbeddedActivities = 0x8000000000uL,
        //
        // Resumo:
        //     Allows for timing out users to prevent them from sending or reacting to messages
        //     in chat and threads, and from speaking in voice and stage channels.
        ModerateUsers = 0x10000000000uL,
        //
        // Resumo:
        //     Allows for viewing role subscription insights.
        ViewCreatorMonetizationAnalytics = 0x20000000000uL,
        //
        // Resumo:
        //     Allows for using soundboard in a voice channel.
        UseSoundboard = 0x40000000000uL,
        //
        // Resumo:
        //     Allows for creating emojis, stickers, and soundboard sounds, and editing and
        //     deleting those created by the current user.
        CreateGuildExpressions = 0x80000000000uL,
        //
        // Resumo:
        //     Allows for creating scheduled events, and editing and deleting those created
        //     by the current user.
        CreateEvents = 0x100000000000uL,
        //
        // Resumo:
        //     Allows the usage of custom soundboard sounds from other servers.
        UseExternalSounds = 0x200000000000uL,
        //
        // Resumo:
        //     Allows sending voice messages.
        SendVoiceMessages = 0x400000000000uL,
        //
        // Resumo:
        //     Allows sending polls.
        SendPolls = 0x2000000000000uL,
        //
        // Resumo:
        //     Allows user-installed apps to send public responses. When disabled, users will
        //     still be allowed to use their apps but the responses will be ephemeral. This
        //     only applies to apps not also installed to the server.
        UseExternalApplications = 0x4000000000000uL
    }
}
