from fastapi import FastAPI, UploadFile, File, Form
from fastapi.responses import FileResponse, JSONResponse
import subprocess
import shutil
import os
import uuid

app = FastAPI()

@app.post("/convert")
async def convert(
    file: UploadFile = File(...),
    output_format: str = Form("pdf"),       # e.g., pdf, docx, xlsx, html, txt, png...
    filter_options: str = Form(None),       # e.g., "FilterData={}" or "writer_pdf_Export:SelectPdfVersion=1"
    output_filename: str = Form(None)       # override default filename
):
    temp_id = str(uuid.uuid4())
    input_ext = os.path.splitext(file.filename)[1]
    input_path = f"/tmp/{temp_id}{input_ext}"

    # Save uploaded file
    with open(input_path, "wb") as f:
        shutil.copyfileobj(file.file, f)

    # Build conversion args
    convert_args = [
        "soffice",
        "--headless",
        "--convert-to",
        f"{output_format}" + ("" if not filter_options else f":{filter_options}"),
        "--outdir",
        "/tmp",
        input_path
    ]

    # Execute LibreOffice
    try:
        subprocess.run(convert_args, check=True)
    except subprocess.CalledProcessError as e:
        return JSONResponse({"error": str(e)}, status_code=500)

    # Determine output file
    base = os.path.splitext(os.path.basename(input_path))[0]
    converted = f"/tmp/{base}.{output_format}"

    if output_filename:
        new_path = f"/tmp/{output_filename}"
        os.rename(converted, new_path)
        converted = new_path

    return FileResponse(converted, filename=os.path.basename(converted))
