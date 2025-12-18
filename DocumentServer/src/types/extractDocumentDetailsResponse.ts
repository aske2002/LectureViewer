export interface PositionedTextChunk {
  text: string;
  x: number;
  y: number;
  width: number;
  height: number;
  startIndexInPage: number; // char offset in the page’s full text
  endIndexInPage: number;
}

export interface PageDetails {
  pageNumber: number;
  width: number;
  height: number;
  text: string;                 // full text of the page (all chunks concatenated)
  chunks: PositionedTextChunk[]; // each chunk has text + coordinates
}

export interface ExtractDocumentDetailsResponse {
  numberOfPages: number;
  title?: string;
  author?: string;
  pages: PageDetails[];
}