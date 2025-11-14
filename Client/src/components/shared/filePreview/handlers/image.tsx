import { FilePreviewHandler } from "@/lib/filePreviews/filePreviewHandler";

export const imagePreviewHandler: FilePreviewHandler = {
  type: "image",
  canHandle: (ext) => ["png", "jpg", "jpeg", "gif", "webp"].includes(ext),
  renderInline: ({ url, name }) => (
    <img
      src={url}
      alt={name}
      className="w-full max-h-48 object-cover rounded-md"
    />
  ),
  renderModal: ({ url, name }) => (
    <div className="overflow-hidden flex flex-1 h-full w-full justify-center bg-black">
      <img src={url} alt={name} className="object-contain" />
    </div>
  ),
};
