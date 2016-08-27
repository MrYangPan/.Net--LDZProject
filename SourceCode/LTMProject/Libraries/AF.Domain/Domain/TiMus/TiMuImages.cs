using System;
using AF.Core;

namespace AF.Domain.Domain.TiMus
{
    /// <summary>
    /// 题目Image表
    /// </summary>
    public class TiMuImages : BaseEntity
    {
        /// <summary>
        /// 题目ID
        /// </summary>
        /// <value>
        /// The tmid.
        /// </value>
        public string Tmid { get; set; }
        
        /// <summary>
        /// 图片路径
        /// </summary>
        /// <value>
        /// The img path.
        /// </value>
        public string ImgPath { get; set; }

        /// <summary>
        /// 图片名称
        /// </summary>
        /// <value>
        /// The name of the img.
        /// </value>
        public string Name { get; set; }
        
        /// <summary>
        /// 上传者ID
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int CustomerId { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        /// <value>
        /// The upload date.
        /// </value>
        public DateTime UploadDate { get; set; }

        /// <summary>
        /// 图片状态
        /// </summary>
        /// <value>
        ///   <c>true</c> if status; otherwise, <c>false</c>.
        /// </value>
        public Boolean Status { get; set; }
    }
}
