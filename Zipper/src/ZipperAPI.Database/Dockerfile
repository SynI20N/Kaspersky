FROM mongo:noble as database
USER root
EXPOSE 8100

COPY start.sh .
RUN chmod +x start.sh
ENTRYPOINT ["./start.sh"]