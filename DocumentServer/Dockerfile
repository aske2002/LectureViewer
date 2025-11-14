FROM ubuntu:22.04

RUN apt-get update && \
    apt-get install -y libreoffice python3 python3-pip && \
    pip3 install fastapi uvicorn python-multipart && \
    apt-get clean

COPY app.py /app/app.py
WORKDIR /app

EXPOSE 8000

CMD ["uvicorn", "app:app", "--host", "0.0.0.0", "--port", "8000"]