namespace AF.Services.Books.Compose
{
    /// <summary>
    ///树 
    /// </summary>
    public class TreeModel
    {
        /// <summary>
        /// 节点编号
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// 父节点名称
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public string Parent { get; set; }

        /// <summary>
        /// 当前结点名称
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; set; }
    }
}
